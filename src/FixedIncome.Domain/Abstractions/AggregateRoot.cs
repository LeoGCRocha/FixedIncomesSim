namespace FixedIncome.Domain.Abstractions;

public class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
{
    private readonly List<DomainEvent> _events = [];
    
    protected AggregateRoot(TId id) : base(id)
    {
    }

    public IReadOnlyList<DomainEvent> GetDomainEvents()
    {
        return _events;
    }

    public void RaiseDomainEvent(DomainEvent dE)
    {
        _events.Add(dE);
    }
}