using System.Linq;
using Hfd.BizLayer.Internals;
using Hfd.Common.Models;
using Hfd.Common.Services;
using Hfd.Identity;
using Hfd.Identity.Model;
using Hfd.Provider.Services;
using Microsoft.AspNetCore.Identity;

namespace Hfd.Backend.Internals;

/// <summary>
/// Реализация по умолчанию
/// </summary>
public class AdminClientFactory(
    IClientFactory<IHfdClient> factory,
    IClientFactory<IProviderClient> providerFactory,
    UserManager<HfdIdentityUser> userManager,
    SignInManager<HfdIdentityUser> signInManager,
    IProviderOptions options) : IAdminClientFactory
{
    public async Task<IProviderClient> CreateClient(Uri uri, int timeout = 0)
    {
        var user = await userManager.FindByNameAsync(IdentitySeedData.AdminPhoneNumber);
        var principal = await signInManager.CreateUserPrincipalAsync(user!);
        var client = factory.CreateUserClient(options.HostUri!);
        var response = await client.Login(
            new LoginInfo() { PhoneNumber = IdentitySeedData.AdminPhoneNumber, Password = IdentitySeedData.AdminPassword, });
        if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
        {
            string cookie = cookies.First();
            var providerClient = providerFactory.CreateUserClient(uri, timeout, cookie);
            return providerClient;
        }
        else
        {
            return providerFactory.CreateUserClient(uri, timeout);
        }
    }
}
