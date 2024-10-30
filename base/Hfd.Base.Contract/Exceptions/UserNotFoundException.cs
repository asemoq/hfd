namespace Hfd.Base.Contract.Exceptions;

public class UserNotFoundException : HfdException
{
	public UserNotFoundException(string message)
		: base(message)
	{
	}
}