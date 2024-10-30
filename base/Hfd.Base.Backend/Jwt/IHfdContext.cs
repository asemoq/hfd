using System.Security.Claims;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Internals;
using Hfd.Base.Contract.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Base.Backend.Jwt;

public interface IHfdContext
{
	CancellationToken RequestAborted { get; }

	IServiceProvider Services { get; }

	Task<ClaimsPrincipal> GetPrincipal(string login, string password);
}

/// <inheritdoc />
public class HfdContext : IHfdContext
{
	private readonly IBizContext _context;

	public HfdContext(IBizContext context)
	{
		_context = context;

		var httpContext = context.Services.GetService<IHttpContextAccessor>().HttpContext;
		if (httpContext != null)
			RequestAborted = httpContext.RequestAborted;
	}

	public CancellationToken RequestAborted { get; } = CancellationToken.None;

	public IServiceProvider Services => _context.Services;

	public async Task<ClaimsPrincipal> GetPrincipal(string username, string password)
	{
		var user = await Services
			.GetRequiredService<IDbStorage>()
			.Get<IUserLoginDbService>()
			.GetOne(username);

		if (user == null)
			throw new Exception(nameof(HfdBaseError.User_Not_Found));

		if (!password.Verify(user?.Password)) return null;

		if (user.Blocked)
			throw new Exception(nameof(HfdBaseError.User_Blocked));


		var claims = new List<Claim>
		{
			new(ClaimTypes.Role, $"{user.Usertype}"),
			new(ClaimTypes.NameIdentifier, user.Username)
		};

		var identities = new List<ClaimsIdentity> { new(claims) };
		return new ClaimsPrincipal(identities);
	}
}