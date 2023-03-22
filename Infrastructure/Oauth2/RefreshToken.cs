using System.Security.Cryptography;
using Infrastructure.Identity;

namespace Infrastructure.Oauth2;

public class RefreshToken
{
    public string Token { get; } = NewToken();

    public DateTime IssuedAt { get; init; } = DateTime.Now;

    public required IdentityUser Identity { get; init; }

    public required Client Client { get; init; }

    private static string NewToken()
    {
        var randomNumber = new byte[32];
        RandomNumberGenerator.Create().GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}