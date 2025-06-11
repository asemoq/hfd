using System.Security.Claims;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Contract.Internals;
using Hfd.Common.Contract.Model;
using Hfd.Common.Contract.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Backend.Internals;

public interface IBackendContext
{
    CancellationToken RequestAborted { get; }

    internal IBizContext BizContext { get; }

    Task<ClaimsPrincipal> GetPrincipal(string login, string password);

    Task<ClaimsPrincipal> GetPrincipal(IUserFull user);
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
    public async Task<ClaimsPrincipal> GetPrincipal(string login, string password)
    {
        var user = await this.BizContext.Services.GetRequiredService<IDbStorage>()
            .GetService<IUserLoginDbService>()
            .GetOne(login);

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (!password.Verify(user.Password))
        {
            return null!;
        }

        if (user.Blocked)
        {
            throw new HfdException(nameof(HfdCoreError.UserBlocked));
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, $"{user.Usertype}"),
            new(ClaimTypes.NameIdentifier, user.Username),
            new(ClaimTypes.UserData, user.UniqueId),
        };

        var identities = new List<ClaimsIdentity> { new(claims) };
        return new ClaimsPrincipal(identities);
    }

    /// <inheritdoc />
    public Task<ClaimsPrincipal> GetPrincipal(IUserFull user)
    {
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (user.Blocked)
        {
            throw new HfdException(nameof(HfdCoreError.UserBlocked));
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, $"{user.Usertype}"),
            new(ClaimTypes.NameIdentifier, user.Username),
            new(ClaimTypes.UserData, user.UniqueId),
        };

        var identities = new List<ClaimsIdentity> { new(claims) };
        return Task.FromResult(new ClaimsPrincipal(identities));
    }
}
