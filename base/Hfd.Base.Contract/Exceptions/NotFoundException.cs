using Microsoft.AspNetCore.Http;

namespace Hfd.Base.Contract.Exceptions;

public class NotFoundException : HfdException
{
	public NotFoundException(string errorText) : base(errorText)
	{
	}

	public override int GetHttpStatus()
	{
		return StatusCodes.Status404NotFound;
	}
}