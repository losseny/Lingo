namespace Application.Common.Interfaces;

public interface ITokenService
{
    Task<string> CreateAccessTokenAsync(string userName);
    Task<string> CreateRefreshTokenAsync(string userName, Guid clientId);
    Task<bool> CheckRefreshTokenAsync(string userName, string token, Guid clientId);
    Task RemoveRefreshTokensAsync(string userName, Guid clientId);
}