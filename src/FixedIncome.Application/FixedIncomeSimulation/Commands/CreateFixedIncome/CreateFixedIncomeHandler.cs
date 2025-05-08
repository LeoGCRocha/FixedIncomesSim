using MediatR;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using FixedIncome.Infrastructure.Persistence.Outbox;
using FixedIncome.Application.Abstractions;
using FixedIncome.Application.Factories.Outbox;
using FixedIncome.Application.FixedIncomeSimulation.DTOs;
using FixedIncome.Domain.FixedIncomeSimulation;

namespace FixedIncome.Application.FixedIncomeSimulation.Commands.CreateFixedIncome;

public class CreateFixedIncomeHandler : IRequestHandler<CreateFixedIncomeCommand, CreateFixedIncomeCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOutboxFactory _outboxFactory;
    private readonly ILogger<CreateFixedIncomeHandler> _logger;

    public CreateFixedIncomeHandler(IUnitOfWork unitOfWork, IOutboxFactory outboxFactory, ILogger<CreateFixedIncomeHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _outboxFactory = outboxFactory;
        _logger = logger;
    }

    public async Task<CreateFixedIncomeCommandResponse> Handle(CreateFixedIncomeCommand request,
        CancellationToken cancellationToken)
    {
        var fixedIncome = new FixedIncomeSim(
            Guid.NewGuid(),
            request.StartDate,
            request.EndDate,
            request.StartAmount,
            request.MonthlyYield,
            request.MonthlyContribution
        );
        
        if (request.Information is not null)
            fixedIncome.SetInformation(request.Information.Title, request.Information.Type);

        var stopwatch = Stopwatch.StartNew();
        
        await _unitOfWork.FixedIncomeRepository.AddAsync(fixedIncome);

        await _unitOfWork.OutboxPatternRepository.AddAsync(
            _outboxFactory.CreateOutboxMessage(EOutboxMessageTypes.Email, fixedIncome.Id));

        await _unitOfWork.OutboxPatternRepository.AddAsync(
            _outboxFactory.CreateOutboxMessage(EOutboxMessageTypes.File, fixedIncome.Id));

        await _unitOfWork.CommitAsync();
        
        stopwatch.Stop();
        
        _logger.LogInformation("Time to run insert on DB {ARG}", stopwatch.ElapsedMilliseconds);
        
        return new CreateFixedIncomeCommandResponse(fixedIncome.Id);
    }
}