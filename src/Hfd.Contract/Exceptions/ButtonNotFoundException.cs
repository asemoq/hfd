using Hfd.Base.Contract.Exceptions;

namespace Hfd.Contract.Exceptions;

public class ButtonNotFoundException : HfdException
{
	public ButtonNotFoundException(string message)
		: base(message)
	{
	}
}