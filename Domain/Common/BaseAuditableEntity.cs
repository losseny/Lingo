namespace Domain.Common;

/// <summary>
/// Defines creational and modified info for the entity's. Likely created by database.
/// This will be implemented by classes that you create.
/// </summary>
public class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}