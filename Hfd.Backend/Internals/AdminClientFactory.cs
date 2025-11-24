using AutoMapper;
using Hfd.BizLayer.Internals;
using Hfd.Common.Backend.Jwt;
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
    IMapper mapper,
    IClientFactory<IProviderClient> factory,
    IJwtHandler handler,
    UserManager<HfdIdentityUser> userManager) : IAdminClientFactory
{
    public async Task<IProviderClient> CreateClient(Uri uri, int timeout = 0)
    {
        var user = await userManager.FindByNameAsync(IdentitySeedData.AdminPhoneNumber);
        var principal = await handler.GetPrincipal(mapper.Map<UserFull>(user));
        var token = handler.CreateToken(principal, TimeSpan.FromDays(1));
        var client = factory.CreateUserClient(uri, timeout);
        client.Authorization = $"Bearer {token}";
        return client;
    }
}
