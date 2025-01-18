using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Enums;

namespace Hfd.Base.Contract.Model;

/// <summary>
///     Карточка пользователя.
/// </summary>
public interface IUserFull
{
	/// <summary>
	///     Identifier.
	/// </summary>
	long Id { get; }

	/// <summary>
	///     ИИН.
	/// </summary>
	public string UniqueId { get; }

	/// <summary>
	///     Тип пользователя (роль учетной записи)
	/// </summary>
	LoginRole Usertype { get; }

	/// <summary>
	///     Логин пользователя, указанный пользователем.
	/// </summary>
	string Username { get; }

	/// <summary>
	///     Пароль (хэш, который хранится в базе).
	/// </summary>
	string Password { get; }

	/// <summary>
	///     ФИО.
	/// </summary>
	string FullName { get; }

	/// <summary>
	///     Признак, что учетная запись заблокирована.
	/// </summary>
	bool Blocked { get; }

	/// <summary>
	///     Выбранный язык по умолчанию.
	/// </summary>
	Language Language { get; }
}