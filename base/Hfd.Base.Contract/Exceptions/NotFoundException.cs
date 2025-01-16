using Microsoft.AspNetCore.Http;

namespace Hfd.Base.Contract.Exceptions;

public class NotFoundException : HfdException
{
	public NotFoundException(params string[] details) : base(details)
	{
	}

	public override int GetHttpStatus()
	{
		return StatusCodes.Status404NotFound;
	}
}