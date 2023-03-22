using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Oauth2;

public static class OAuth2ModelBuilderExtensions
{
    public static ModelBuilder ConfigureOAuth2(this ModelBuilder builder)
    {
        builder.Entity<RefreshToken>()
            .HasKey(refreshToken => refreshToken.Token);

        builder.Entity<RefreshToken>()
            .Property(refreshToken => refreshToken.Token)
            .HasMaxLength(44);

        return builder;
    }
}