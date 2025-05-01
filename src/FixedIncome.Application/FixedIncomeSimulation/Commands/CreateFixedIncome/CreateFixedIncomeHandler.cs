using System.Diagnostics;
using FixedIncome.Application.FixedIncomeSimulation.DTOs;
using FixedIncome.Domain.Common.Abstractions;
using FixedIncome.Domain.FixedIncomeSimulation;
using FixedIncome.Infrastructure.Persistence.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FixedIncome.Application.FixedIncomeSimulation.Commands.CreateFixedIncome;

public class CreateFixedIncomeHandler : IRequestHandler<CreateFixedIncomeCommand, CreateFixedIncomeCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateFixedIncomeHandler> _logger;

    public CreateFixedIncomeHandler(IUnitOfWork unitOfWork, ILogger<CreateFixedIncomeHandler> logger)
    {
        _unitOfWork = unitOfWork;
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
        await _unitOfWork.CommitAsync();
        
        stopwatch.Stop();
        
        _logger.LogInformation("Time to run insert on DB {ARG}", stopwatch.ElapsedMilliseconds);
        
        return new CreateFixedIncomeCommandResponse(fixedIncome.Id);
    }
}