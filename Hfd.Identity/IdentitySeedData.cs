using System;
using System.Linq;
using System.Threading.Tasks;
using Hfd.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Identity;

/// <summary>
/// Provides methods to ensure the identity data is populated in the database.
/// </summary>
public static class IdentitySeedData
{
    private const string AdminUserName = "Admin";
    private const string AdminPhoneNumber = "555-1234";
    private const string AdminUniqueId = "77777777777";
    private const string AdminFullName = "Admin Full Name";
    private const string AdminPassword = "Secret123$";
    private const string AdminRole = "Administrator";
    private const string NurseRole = "Nurse";

    /// <summary>
    /// Ensures that the identity data is populated in the database.
    /// </summary>
    /// <param name="serviceProvider">The service provider to use for resolving dependencies.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static Task EnsurePopulated(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        return EnsurePopulatedAsync(serviceProvider);
    }

    private static async Task EnsurePopulatedAsync(IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        HfdIdentityDbContext context = serviceScope.ServiceProvider.GetRequiredService<HfdIdentityDbContext>();

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }

        RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roleTypes = Enum.GetValues(typeof(LoginRole)).Cast<LoginRole>();

        foreach (var roleType in roleTypes)
        {
            var role = await roleManager.FindByNameAsync(roleType.ToString());
            if (role is null)
            {
                role = new IdentityRole(roleType.ToString());
                await roleManager.CreateAsync(role);
            }
        }

        UserManager<HfdIdentityUser>
            userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<HfdIdentityUser>>();
        var user = await userManager.FindByNameAsync(AdminUserName);

        if (user is null)
        {
            user = new HfdIdentityUser()
            {
                UserName = AdminUserName, Iin = AdminUniqueId, FullName = AdminFullName, PhoneNumber = AdminPhoneNumber,
            };
            await userManager.CreateAsync(user, AdminPassword);
        }
        else
        {
            user.UserName = AdminUserName;
            user.Iin = AdminUniqueId;
            user.FullName = AdminFullName;
            user.PhoneNumber = AdminPhoneNumber;
            await userManager.UpdateAsync(user);
            await userManager.ChangePasswordAsync(user, user.PasswordHash!, AdminPassword);
        }

        var roles = await userManager.GetRolesAsync(user);
        if (roles.All(x => x != AdminRole))
        {
            await userManager.AddToRoleAsync(user, AdminRole);
            await userManager.AddToRoleAsync(user, NurseRole);
        }
    }
}
