using FixedIncome.Application.Mediator;

namespace FixedIncome.Application.FixedIncomeSimulation.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
    public Guid Id { get; }
}