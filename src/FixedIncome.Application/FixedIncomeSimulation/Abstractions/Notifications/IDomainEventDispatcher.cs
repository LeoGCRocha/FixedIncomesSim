using FixedIncome.Domain.Common.Abstractions;

namespace FixedIncome.Application.FixedIncomeSimulation.Abstractions.Notifications;

public interface IDomainEventDispatcher
{
    public Task DispatchAsync(DomainEvent domainEvent);
}