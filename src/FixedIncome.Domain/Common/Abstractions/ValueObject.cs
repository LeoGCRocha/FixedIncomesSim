namespace FixedIncome.Domain.Common.Abstractions;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object> GetAtomicValues();
    
    public override bool Equals(object? other)
    {
        return other is not null && Equals(other as ValueObject);
    }
    
    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Select(x => x.GetHashCode())
            .Aggregate((x, y) => x ^ y);
    }
    
    public bool Equals(ValueObject? other)
    {
        if (other is null)
            return false;

        if (GetType() != other.GetType())
            return false;

        var thisValues = GetAtomicValues().GetEnumerator();
        var otherValues = other.GetAtomicValues().GetEnumerator();

        while (thisValues.MoveNext() && otherValues.MoveNext())
        {
            if ((thisValues.Current is not null && otherValues.Current is not null)
                && (!thisValues.Current.Equals(otherValues.Current)))
                return false;
        }

        return (!thisValues.MoveNext() && !otherValues.MoveNext());
    }

    public static bool operator ==(ValueObject a, ValueObject b)
    {
        return Equals(a, b);
    }

    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !Equals(a, b);
    }
}