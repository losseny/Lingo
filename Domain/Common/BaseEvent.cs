using MediatR;

namespace Domain.Common;

/// <summary>
/// Defines our own notification by implementing <see cref="INotification"/>
/// Will be used to define CRUD events.
/// Maybe will be handled by our own handler.
/// </summary>
public abstract class BaseEvent : INotification
{
    
}