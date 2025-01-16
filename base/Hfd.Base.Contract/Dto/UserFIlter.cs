using Hfd.Base.Contract.Enums;
using Hfd.Base.Contract.Model;

namespace Hfd.Base.Contract.Dto;

public class UserFilter : SkipTakeRq
{
	/// <summary>
	///     Тип учетной записи.
	/// </summary>
	public LoginRole? Usertype { get; set; }

	/// <summary>
	///     Выбранный язык по умолчанию.
	/// </summary>
	public Language? Language { get; set; }

	/// <summary>
	///     Признак, что учетная запись заблокирована.
	/// </summary>
	public bool? Blocked { get; set; }

	/// <summary>
	///     Признак, что учетная запись удалена.
	/// </summary>
	public bool? Deleted { get; set; }
}