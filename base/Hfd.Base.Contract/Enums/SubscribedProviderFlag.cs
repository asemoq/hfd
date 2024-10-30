namespace Hfd.Base.Contract.Enums;

[Flags]
public enum SubscribedProviderFlag : long
{
	None = 0,

	/// <summary>
	///     Whatsapp
	/// </summary>
	Whatsapp = 1L << 0, // 1

	/// <summary>
	///     Telegram
	/// </summary>
	Telegram = 1L << 1 // 2
}

public static class SubscribedProviderFlagExtensions
{
	public static SubscribedProviderFlag With(this SubscribedProviderFlag value, SubscribedProviderFlag flag)
	{
		return value | flag;
	}

	public static SubscribedProviderFlag Without(this SubscribedProviderFlag value, SubscribedProviderFlag flag)
	{
		return value & ~flag;
	}

	public static SubscribedProviderFlag SetFlag(this SubscribedProviderFlag value, SubscribedProviderFlag flag,
		bool set)
	{
		return set ? value.With(flag) : value.Without(flag);
	}
}