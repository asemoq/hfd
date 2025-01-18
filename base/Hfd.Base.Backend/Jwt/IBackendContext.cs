using System.Collections.Generic;
using System.Security.Claims;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Internals;

namespace Hfd.Base.Backend.Jwt;

/// <summary>
/// 
/// </summary>
public interface IBackendContext
{
	CancellationToken RequestAborted { get; }

	IServiceProvider Services { get; }
	internal IBizContext BizContext { get; }

	Task<ClaimsPrincipal> GetPrincipal(string login, string password);
}

/// <inheritdoc />
public class BackendContext : IBackendContext
{
	public IBizContext BizContext { get; }

	public BackendContext(IBizContext context)
	{
		BizContext = context;

		var httpContext = context.Services.GetService<IHttpContextAccessor>().HttpContext;
		if (httpContext != null)
			RequestAborted = httpContext.RequestAborted;
	}

	public CancellationToken RequestAborted { get; } = CancellationToken.None;

	public IServiceProvider Services => BizContext.Services;

	public async Task<ClaimsPrincipal> GetPrincipal(string username, string password)
	{
		var user = await Services
			.GetRequiredService<IDbStorage>()
			.Get<IUserLoginDbService>()
			.GetOne(username);

		if (user == null)
			throw new UserNotFoundException();

		if (!password.Verify(user?.Password)) return null;

		if (user.Blocked)
			throw new HfdException(nameof(HfdBaseError.User_Blocked));


		var claims = new List<Claim>
		{
			new(ClaimTypes.Role, $"{user.Usertype}"),
			new(ClaimTypes.NameIdentifier, user.Username),
			new(ClaimTypes.UserData, user.UniqueId)
		};

		var identities = new List<ClaimsIdentity> { new(claims) };
		return new ClaimsPrincipal(identities);
	}
}