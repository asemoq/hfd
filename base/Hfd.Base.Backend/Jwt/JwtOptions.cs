using Microsoft.IdentityModel.Tokens;

namespace Hfd.Base.Backend.Jwt;

internal sealed class JwtOptions
{
	public SigningCredentials SigningCredentials { get; set; }

	public string Issuer { get; set; }

	public string Audience { get; set; }
}