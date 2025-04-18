using FixedIncome.Application.FixedIncomeSimulation.DTOs;
using FixedIncome.Domain.Common.ValueObjects;
using MediatR;

namespace FixedIncome.Application.FixedIncomeSimulation.Commands.CreateFixedIncome;

public class CreateFixedIncomeCommand : IRequest<CreateFixedIncomeCommandResponse>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal StartAmount { get; set; }
    public decimal MonthlyYield { get; set; }
    public decimal MonthlyContribution { get; set; }
    public InvestmentInformation? Information { get; set; }
}