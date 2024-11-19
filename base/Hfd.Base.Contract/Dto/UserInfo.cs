using Hfd.Base.Contract.Enums;

namespace Hfd.Base.Contract.Dto;

/// <summary>
///     User model.
/// </summary>
public class UserInfo
{
	/// <summary>
	///     Логин пользователя, указанный пользователем.
	/// </summary>
	public string Username { get; set; }

	/// <summary>
	///     ИИН.
	/// </summary>
	public string Iin { get; set; }

	/// <summary>
	///     ФИО.
	/// </summary>
	public string FullName { get; set; }

	/// <summary>
	///     Выбранный язык по умолчанию.
	/// </summary>
	public Language Language { get; set; }

	/// <summary>
	///     Пароль.
	/// </summary>
	public string Password { get; set; }

	/// <summary>
	///     Тип создаваемого пользователя
	/// </summary>
	public LoginRole Usertype { get; set; }
}

public class ChangePasswordInfo
{
	/// <summary>
	///     Пароль.
	/// </summary>
	public string OldPassword { get; set; }

	/// <summary>
	///     Пароль.
	/// </summary>
	public string NewPassword { get; set; }
}