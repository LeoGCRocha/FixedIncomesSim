using FixedIncome.Application.FixedIncomeSimulation.DTOs;
using FixedIncome.Domain.Common.Abstractions;
using FixedIncome.Domain.FixedIncomeSimulation;
using MediatR;
using Microsoft.VisualBasic;

namespace FixedIncome.Application.FixedIncomeSimulation.Commands.CreateFixedIncome;

public class CreateFixedIncomeHandler : IRequestHandler<CreateFixedIncomeCommand, CreateFixedIncomeCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateFixedIncomeHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
        
        await _unitOfWork.FixedIncomeRepository.AddAsync(fixedIncome);
        return new CreateFixedIncomeCommandResponse(fixedIncome.Id);
    }
}