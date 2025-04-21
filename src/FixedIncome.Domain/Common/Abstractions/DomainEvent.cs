namespace FixedIncome.Domain.Common.Abstractions;

public class DomainEvent : Entity<Guid>
{
    public string Type { get; init; }
    public DateTime OccuredOn { get; protected set; }= DateTime.Now;
    
    protected DomainEvent(Guid id, string type) : base(id)
    {
        Type = type;
    }
}