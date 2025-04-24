using FixedIncome.Infrastructure.DomainEvents.Abstractions;
using FixedIncome.Domain.Common.Abstractions;
using MediatR;

namespace FixedIncome.Infrastructure.DomainEvents;

public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public async Task DispatchEvents(List<DomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}