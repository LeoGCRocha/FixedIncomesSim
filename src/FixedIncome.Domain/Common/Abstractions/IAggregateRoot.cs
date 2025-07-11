namespace FixedIncome.Domain.Common.Abstractions;

public interface IAggregateRoot
{
    public IReadOnlyList<DomainEvent> GetDomainEvents();

    public void RaiseDomainEvent(DomainEvent dE);

    public void ClearDomainEvents();
}