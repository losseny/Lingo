using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

internal static class IdentityModelBuilderExtensions
{
    public static ModelBuilder ConfigureIdentity(this ModelBuilder builder)
    {
        builder.Entity<IdentityUser>()
            .ToTable("IdentityUsers")
            .HasQueryFilter(identityUser => !identityUser.DeletedAt.HasValue);

        builder.Entity<IdentityRole>()
            .ToTable("IdentityRoles");

        builder.Entity<IdentityUserRole<string>>()
            .ToTable("IdentityUserRoles");

        builder.Entity<IdentityUserClaim<string>>()
            .ToTable("IdentityUserClaims");

        builder.Entity<IdentityUserLogin<string>>()
            .ToTable("IdentityUserLogins");

        builder.Entity<IdentityRoleClaim<string>>()
            .ToTable("IdentityRoleClaims");

        builder.Entity<IdentityUserToken<string>>()
            .ToTable("IdentityUserTokens");

        return builder;
    }
}