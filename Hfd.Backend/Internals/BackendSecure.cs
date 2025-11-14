using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Hfd.Common.Backend.Jwt;
using Hfd.Common.Contract;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Models;
using Hfd.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Hfd.Backend.Internals;

internal sealed class BackendSecure(
    IBackendContext context,
    IMapper mapper,
    UserManager<HfdIdentityUser> userManager,
    ILogger<BackendSecure> log) : IBackendSecure
{
    public async Task<UserFull> Verify(ClaimsPrincipal principal)
    {
        try
        {
            var id = principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var username = principal.Claims.Single(c => c.Type == ClaimTypes.Name).Value;
            var uniqueId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.UserData);
            var user = await userManager.FindByIdAsync(id) ?? throw new UserNotFoundException(username);
            if (user.Iin == uniqueId?.Value)
            {
                return mapper.Map<UserFull>(user);
            }
        }
        catch (Exception)
        {
            log.Error("Не удалось проверить принципала.");
        }

        throw new UnauthorizedException();
    }

    public async Task<ClaimsPrincipal> GetPrincipal(UserFull user)
    {
        var dbUser = await userManager.FindByIdAsync(user.Id) ?? throw new UserNotFoundException(user.UserName!);
        var roles = await userManager.GetRolesAsync(dbUser);
        return await context.GetPrincipal(dbUser, roles);
    }
}
