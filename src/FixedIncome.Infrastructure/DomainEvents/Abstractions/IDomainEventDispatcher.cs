using FixedIncome.Domain.Common.Abstractions;

namespace FixedIncome.Infrastructure.DomainEvents.Abstractions;

public interface IDomainEventDispatcher
{
    public Task DispatchEvents(List<DomainEvent> domainEvents);
}