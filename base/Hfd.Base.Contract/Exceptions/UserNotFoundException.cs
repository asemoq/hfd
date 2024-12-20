using Microsoft.AspNetCore.Http;

namespace Hfd.Base.Contract.Exceptions;

public class UserNotFoundException : HfdException
{
	public UserNotFoundException()
		: base("Не найдена учетная запись.")
	{
	}

	public override int GetHttpStatus()
	{
		return StatusCodes.Status400BadRequest;
	}
}