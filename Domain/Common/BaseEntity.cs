using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;

/// <summary>
/// Defines Base for all our entities
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }

    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped] // <--- Is like java annotation that says that this property should not be mapped by database mapping. So ignore this property when you are mapping this class. 
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}