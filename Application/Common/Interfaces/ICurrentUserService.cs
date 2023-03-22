namespace Application.Common.Interfaces;

/// <summary>
/// defines the current user by returning the id of current user
/// </summary>
public interface ICurrentUserService
{
    int UserId { get; }
}   