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

internal sealed class BackendSecure(IMapper mapper, UserManager<HfdIdentityUser> userManager, ILogger<BackendSecure> log) : IBackendSecure
{
    public async Task<UserFull> Verify(ClaimsPrincipal principal)
    {
        try
        {
            var id = principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var username = principal.Claims.Single(c => c.Type == ClaimTypes.Name).Value;
            var uniqueId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.UserData);
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

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
}
