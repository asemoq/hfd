using System.Linq;
using System.Security.Claims;
using Hfd.Common.Contract.Services;
using Hfd.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Backend.Internals;

public interface IBackendContext
{
    CancellationToken RequestAborted { get; }

    internal IBizContext BizContext { get; }

    Task<ClaimsPrincipal> GetPrincipal(HfdIdentityUser user, IEnumerable<string> roles);
}

/// <inheritdoc />
public class BackendContext : IBackendContext
{
    public BackendContext(IBizContext context)
    {
        this.BizContext = context;

        var httpContext = context.Services.GetService<IHttpContextAccessor>()?.HttpContext;
        if (httpContext != null)
        {
            this.RequestAborted = httpContext.RequestAborted;
        }
    }

    /// <inheritdoc />
    public IBizContext BizContext { get; }

    /// <inheritdoc />
    public CancellationToken RequestAborted { get; } = CancellationToken.None;

    /// <inheritdoc />
    public Task<ClaimsPrincipal> GetPrincipal(HfdIdentityUser user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new("AspNet.Identity.SecurityStamp", user.SecurityStamp!),
            new("amr", "pwd"),
            new(ClaimTypes.UserData, user.Iin!),
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        var identities = new List<ClaimsIdentity> { new(claims) };
        return Task.FromResult(new ClaimsPrincipal(identities));
    }
}
