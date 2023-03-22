using Application.Common.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Oauth2;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace Infrastructure;

/// <summary>
/// This is where Dependency-Injection happens
/// </summary>
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<AuditableEntityInterceptor>(); // Adds a service in scope. Dependency-Injection
        //services.AddScoped<SoftDeletableEntityInterceptor>();
        
        // adds the config of db context to service
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")!,
                builder =>
                {
                    builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                })
        );
        
        // Adds a database context to the services
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        //services.AddScoped<ApplicationDbContextInitialiser>();
        
        services.AddOptions<OAuth2Options>()
            .Bind(configuration.GetSection("Authentication").GetSection(OAuth2Options.OAuth2))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        // config for Identity
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 8;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
        });
        
        services.AddTransient<IdentityService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}