namespace Hfd.Base.Contract.Exceptions;

public class HfdException : Exception
{
	public HfdException(string errorText) : base(errorText)
	{
	}

	public virtual int GetHttpStatus()
	{
		return 400;
	}
}