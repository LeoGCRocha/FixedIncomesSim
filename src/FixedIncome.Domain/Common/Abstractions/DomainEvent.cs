namespace FixedIncome.Domain.Common.Abstractions;

public class DomainEvent : Entity<Guid>
{
    public string Type { get; init; }
    
    protected DomainEvent(Guid id, string type) : base(id)
    {
        Type = type;
    }
}