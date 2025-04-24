namespace FixedIncome.Domain.Common.Abstractions;

public class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
{
    private List<DomainEvent> _events = [];
    
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

    public void ClearDomainEvents()
    {
        _events = [];
    }
}