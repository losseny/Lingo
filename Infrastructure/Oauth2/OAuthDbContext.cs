using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Oauth2;

public class OAuthDbContext<TIdentityUser> : IdentityDbContext<TIdentityUser> where TIdentityUser : IdentityUser
{
    public OAuthDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Client> Clients => Set<Client>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
}