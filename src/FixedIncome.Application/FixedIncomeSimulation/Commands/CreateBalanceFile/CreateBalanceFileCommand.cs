using MediatR;

namespace FixedIncome.Application.FixedIncomeSimulation.Commands.CreateBalanceFile;

public class CreateBalanceFileCommand : IRequest
{
    public Guid Id { get; set; }
}