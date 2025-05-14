
using FixedIncome.Application.Mediator;

namespace FixedIncome.Application.FixedIncomeSimulation.Commands.DeleteFixedIncome;

public class DeleteFixedIncomeCommand : IRequest
{
    public Guid Id { get; set; }
}