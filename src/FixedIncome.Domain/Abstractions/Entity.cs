namespace FixedIncome.Domain.Abstractions;

public class Entity<TId> : IEquatable<Entity<TId>>
{
    public TId Id { get; protected set; }

    protected Entity(TId id)
    {
        Id = id;
    }

    public override bool Equals(object? other)
    {
        return other is not null && Equals(other as Entity<TId>);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GetType(), Id);
    }

    public bool Equals(Entity<TId>? other)
    {
        if (ReferenceEquals(this, other))
            return true;
        
        if (other is null)
            return false;

        return GetType() == other.GetType() && EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public static bool operator ==(Entity<TId> a, Entity<TId> b)
    {
        return Equals(a, b);
    }

    public static bool operator !=(Entity<TId> a, Entity<TId> b)
    {
        return !Equals(a, b);
    }
}