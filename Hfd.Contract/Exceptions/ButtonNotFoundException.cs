using Hfd.Common.Contract.Exceptions;

namespace Hfd.Contract.Exceptions;

public class ButtonNotFoundException : HfdException
{
    public ButtonNotFoundException(string message)
        : base(message)
    {
    }

    public ButtonNotFoundException()
    {
    }

    public ButtonNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
