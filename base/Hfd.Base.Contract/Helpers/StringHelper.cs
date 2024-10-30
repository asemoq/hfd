using System.Text;

namespace Hfd.Base.Contract.Helpers;

public static class StringHelper
{
	public static string ToDefaultIfEmpty(this string t, string @default = "")
	{
		return string.IsNullOrEmpty(t) ? @default : t;
	}

	public static bool HasValue(this string t, string @default = "")
	{
		return t.ToDefaultIfEmpty() != @default;
	}

	public static string ToGuidIfEmpty(this string t)
	{
		return string.IsNullOrEmpty(t) ? Guid.NewGuid().ToString("N") : t;
	}

	public static string ToLowerFirstChar(this string t)
	{
		if (!string.IsNullOrEmpty(t) && char.IsUpper(t[0]))
		{
			var sb = new StringBuilder(t);
			sb[0] = char.ToLower(sb[0]);
			t = sb.ToString();
		}

		return t;
	}
}