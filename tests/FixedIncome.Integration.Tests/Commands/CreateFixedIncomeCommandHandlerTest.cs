using FixedIncome.Application.Factories.Outbox;
using FixedIncome.Domain.Common.Enums;
using FixedIncome.Domain.Common.ValueObjects;
using FixedIncome.Integration.Tests.Fixture;
using FixedIncome.Application.FixedIncomeSimulation.Commands.CreateFixedIncome;
using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Domain.FixedIncomeSimulation.FixedIncomeOrders;
using FixedIncome.Domain.FixedIncomeSimulation.Repository;
using FixedIncome.Infrastructure.Persistence.Abstractions;
using FixedIncome.Infrastructure.Persistence.Outbox;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FixedIncome.Integration.Tests.Commands;

[Collection("FixedIncomeIntegrationTests")]
public class CreateFixedIncomeCommandHandlerTest
{
    private readonly FixedIncomeFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutboxFactory _outboxFactory;
    private readonly CreateFixedIncomeHandler _commandHandler;

    public CreateFixedIncomeCommandHandlerTest()
    {
        _fixture = new();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _outboxFactory = Substitute.For<IOutboxFactory>();
        _commandHandler = new CreateFixedIncomeHandler(_unitOfWork, _outboxFactory, Substitute.For<ILogger<CreateFixedIncomeHandler>>());
    }

    [Fact(DisplayName = "Should Create FixedIncome And Write To Outbox")]
    public async Task Should_Create_FixedIncome_And_Write_To_Outbox()
    {
        // Arrange
        var fixedIncomeRepositoryMock = Substitute.For<IFixedIncomeRepository>();
        var outboxPatternRepositoryMock = Substitute.For<IOutboxPatternRepository>();

        _unitOfWork.FixedIncomeRepository.Returns(fixedIncomeRepositoryMock);
        _unitOfWork.OutboxPatternRepository.Returns(outboxPatternRepositoryMock);
        
        _outboxFactory.CreateOutboxMessage(EOutboxMessageTypes.Email, Arg.Any<Guid>())
            .Returns(_ => OutboxMessage.OutboxMessageBuilder(EOutboxMessageTypes.Email.ToString(), ""));
        
        _outboxFactory.CreateOutboxMessage(EOutboxMessageTypes.File, Arg.Any<Guid>())
            .Returns(_ => OutboxMessage.OutboxMessageBuilder(EOutboxMessageTypes.File.ToString(), ""));
        
        var request = new CreateFixedIncomeCommand
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddYears(1),
            StartAmount = 10000,
            MonthlyYield = 1.22m,
            MonthlyContribution = 3000,
            Information = new InvestmentInformation(title: "FixedIncome", type: EFixedIncomeOrderType.Cdb)
        };
        
        // Act
        await _commandHandler.Handle(request, CancellationToken.None);
        
        // Assert
        await _unitOfWork.FixedIncomeRepository.Received(1).AddAsync(Arg.Any<FixedIncomeSim>());
        
        await _unitOfWork.OutboxPatternRepository.Received(1)
            .AddAsync(Arg.Is<OutboxMessage>(x => x.Type == EOutboxMessageTypes.Email.ToString()));
        
        await _unitOfWork.OutboxPatternRepository.Received(1)
            .AddAsync(Arg.Is<OutboxMessage>(x => x.Type == EOutboxMessageTypes.File.ToString()));
    }
}