using System.Diagnostics;
using Microsoft.Extensions.Logging;
using FixedIncome.Infrastructure.Persistence.Outbox;
using FixedIncome.Application.Abstractions;
using FixedIncome.Application.Factories.Outbox;
using FixedIncome.Application.FixedIncomeSimulation.DTOs;
using FixedIncome.Application.Mediator;
using FixedIncome.Domain.FixedIncomeSimulation;

namespace FixedIncome.Application.FixedIncomeSimulation.Commands.CreateFixedIncome;

public class CreateFixedIncomeHandler(
    IUnitOfWork unitOfWork,
    IOutboxFactory outboxFactory,
    ILogger<CreateFixedIncomeHandler> logger)
    : IRequestHandler<CreateFixedIncomeCommand, CreateFixedIncomeCommandResponse>
{
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

        await unitOfWork.BulkCopyFixedIncomeSim(fixedIncome);
        
        await unitOfWork.OutboxPatternRepository.AddAsync(
            outboxFactory.CreateOutboxMessage(EOutboxMessageTypes.Email, fixedIncome.Id));
        
        await unitOfWork.OutboxPatternRepository.AddAsync(
            outboxFactory.CreateOutboxMessage(EOutboxMessageTypes.File, fixedIncome.Id));
        
        await unitOfWork.CommitAsync();
        
        stopwatch.Stop();
        
        logger.LogInformation("Time to run insert on DB {ARG}", stopwatch.ElapsedMilliseconds);
        
        return new CreateFixedIncomeCommandResponse(fixedIncome.Id);
    }
}