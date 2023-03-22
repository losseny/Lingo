namespace Domain.Common;

/// <summary>
/// A ValueObject is simply is a object created simply to hold complex data. Mostly of its parent class.
/// There are two main characteristics for value objects:
/// 1. They have no identity.
/// 2. They are immutable. So the values cannot be changed when created
/// </summary>
public abstract class ValueObject
{
    protected static bool EqualOperator(ValueObject? left, ValueObject? right) =>
        !(left is null ^ right is null) && left?.Equals(right!) != false;

    protected static bool NotEqualOperator(ValueObject? left, ValueObject? right) => !EqualOperator(left, right);

    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj) =>
        obj != null && obj.GetType() == GetType() && GetEqualityComponents().SequenceEqual(((ValueObject)obj).GetEqualityComponents());

    public override int GetHashCode() =>
        GetEqualityComponents()
            .Select(obj => obj != null ? obj.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
}