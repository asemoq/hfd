using Microsoft.AspNetCore.Http;

namespace Hfd.Base.Contract.Exceptions;

public class ForbiddenException : HfdException
{
	public ForbiddenException()
		: base("Действие запрещено")
	{
	}

	public override int GetHttpStatus()
	{
		return StatusCodes.Status403Forbidden;
	}
}