using Microsoft.AspNetCore.Http;

namespace Hfd.Base.Contract.Exceptions;

public class UnauthorizedException : HfdException
{
	public UnauthorizedException(string errorText = "")
		: base(!string.IsNullOrEmpty(errorText) ? errorText : "Требуется аутентификация")
	{
	}

	public override int GetHttpStatus()
	{
		return StatusCodes.Status401Unauthorized;
	}
}