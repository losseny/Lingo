namespace Domain.Common;

/// <summary>
/// Defines info when entity was deleted
/// </summary>
public class BaseSoftDeletableEntity : BaseEntity
{
    public DateTime? DeletedAt { get; set; }
}