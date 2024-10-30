using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace Hfd.Base.Backend.Jwt;

public interface IJwtHandler
{
	/// <summary>
	///     Создает токен пользователя.
	/// </summary>
	/// <param name="principal">Пользователь.</param>
	/// <param name="period">Время жизни токена.</param>
	/// <returns>JWT</returns>
	string CreateToken(ClaimsPrincipal principal, TimeSpan period);
}

internal sealed class JwtHandler : IJwtHandler
{
	private readonly JwtOptions _options;

	public JwtHandler(IOptions<JwtOptions> options)
	{
		_options = options.Value;
	}

	public string CreateToken(ClaimsPrincipal principal, TimeSpan period)
	{
		var dateFrom = DateTime.Now;
		var dateTo = dateFrom.Add(period);

		var token = new JwtSecurityToken(_options.Issuer, _options.Audience, principal.Claims, dateFrom, dateTo,
			_options.SigningCredentials);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}