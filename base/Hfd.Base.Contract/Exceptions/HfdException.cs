namespace Hfd.Base.Contract.Exceptions;

public class HfdException : Exception
{
	public HfdException(params string[] details)
		: base("HFD error")
	{
		Details.AddRange(details);
	}

	public virtual int GetHttpStatus()
	{
		return 400;
	}

	/// <summary>
	/// Детали ошибки.
	/// </summary>
	public List<string> Details { get; } = [];
}