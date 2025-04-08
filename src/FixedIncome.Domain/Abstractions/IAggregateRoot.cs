namespace FixedIncome.Domain.Abstractions;

public interface IAggregateRoot
{
    public IReadOnlyList<DomainEvent> GetDomainEvents();

    public void RaiseDomainEvent(DomainEvent dE);
}