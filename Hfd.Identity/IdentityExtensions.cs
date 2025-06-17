using Hfd.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Identity;

/// <summary>
/// Provides extension methods for setting up and interacting with a database layer within an ASP.NET Core application.
/// </summary>
public static class IdentityExtensions
{
    /// <summary>
    /// Registers database layer services within the application's service collection.
    /// </summary>
    /// <param name="services">The IServiceCollection to extend.</param>
    /// <param name="configuration">The configuration object to use for retrieving connection strings.</param>
    /// <returns>The modified IServiceCollection with database layer services registered.</returns>
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
            })
            .AddIdentityCookies();
        services.AddIdentityCore<HfdIdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<HfdIdentityDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
        services.AddDbContext<HfdIdentityDbContext>(options
            => options.UseNpgsql(configuration.GetConnectionString("IdentityConnection")));
        services.AddDatabaseDeveloperPageExceptionFilter();
        return services;
    }
}
