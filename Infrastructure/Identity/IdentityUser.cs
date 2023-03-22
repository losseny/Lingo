using Infrastructure.Oauth2;

namespace Infrastructure.Identity;

public class IdentityUser : Microsoft.AspNetCore.Identity.IdentityUser
{
    public DateTime? DeletedAt { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}