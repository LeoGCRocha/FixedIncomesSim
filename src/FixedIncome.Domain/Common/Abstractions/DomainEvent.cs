namespace FixedIncome.Domain.Common.Abstractions;

public class DomainEvent : Entity<Guid>
{
    public string Type { get; init; }
    public DateTime OccuredWhen = DateTime.Now;
    
    protected DomainEvent(Guid id, string type) : base(id)
    {
        Type = type;
    }
}