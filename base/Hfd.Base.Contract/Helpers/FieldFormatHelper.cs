using System.Text.RegularExpressions;

namespace Hfd.Base.Contract.Helpers;

public partial class FieldFormatHelper
{
	[GeneratedRegex(@"^\d{12}$")]
	private static partial Regex RegexIin();

	[GeneratedRegex(@"^[+][7][\s]?[(]?(\d{3})[)]?[\s]?(\d{3})[-\s]?(\d{4})")]
	private static partial Regex RegexPhoneNumber();

	public static string GetFormattedPhoneNumber(string phoneNumber, bool ui = false)
	{
		if (ui)
		{
			return RegexPhoneNumber().Replace(phoneNumber,
				m => "+7 (" + m.Groups[1].Value + ") " + m.Groups[2].Value + "-" + m.Groups[3].Value);
		}
		else
		{
			return RegexPhoneNumber().Replace(phoneNumber,
				m => "+7" + m.Groups[1].Value + m.Groups[2].Value + m.Groups[3].Value);
		}
	}

	public static bool IsValidPhoneNumber(string phoneNumber)
	{
		return RegexPhoneNumber().IsMatch(phoneNumber);
	}

	public static bool IsValidIin(string iin)
	{
		return RegexIin().IsMatch(iin);
	}
}