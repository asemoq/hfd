using System.Security.Claims;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Internals;
using Hfd.Base.Contract.Model;
using Hfd.Base.Contract.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Backend.Internals;

/// <summary>
/// 
/// </summary>
public interface IBackendContext
{
	/// <summary>
	/// 
	/// </summary>
	CancellationToken RequestAborted { get; }

	internal IBizContext BizContext { get; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="login"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	Task<ClaimsPrincipal> GetPrincipal(string login, string password);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="user"></param>
	/// <returns></returns>
	Task<ClaimsPrincipal> GetPrincipal(IUserFull user);
}

/// <inheritdoc />
public class BackendContext : IBackendContext
{
	/// <inheritdoc />
	public IBizContext BizContext { get; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="context"></param>
	public BackendContext(IBizContext context)
	{
		BizContext = context;

		var httpContext = context.Services.GetService<IHttpContextAccessor>().HttpContext;
		if (httpContext != null)
			RequestAborted = httpContext.RequestAborted;
	}

	/// <inheritdoc />
	public CancellationToken RequestAborted { get; } = CancellationToken.None;

	/// <inheritdoc />
	public async Task<ClaimsPrincipal> GetPrincipal(string username, string password)
	{
		var user = await BizContext.Services
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

	/// <inheritdoc />
	public Task<ClaimsPrincipal> GetPrincipal(IUserFull user)
	{
		if (user == null)
			throw new UserNotFoundException();

		if (user.Blocked)
			throw new HfdException(nameof(HfdBaseError.User_Blocked));


		var claims = new List<Claim>
		{
			new(ClaimTypes.Role, $"{user.Usertype}"),
			new(ClaimTypes.NameIdentifier, user.Username),
			new(ClaimTypes.UserData, user.UniqueId)
		};

		var identities = new List<ClaimsIdentity> { new(claims) };
		return Task.FromResult(new ClaimsPrincipal(identities));
	}
}