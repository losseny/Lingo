using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<bool> CheckPasswordSignInAsync(string userName, string password);

    Task<IList<string>> GetRolesAsync(string userName);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<Result> DeleteUserAsync(string userId);
}