using Hfd.Base.Contract.Enums;

namespace Hfd.Base.Contract.Dto;

public class UserFull
{
	/// <summary>
	///     Идентификатор учетной записи.
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	///     ИИН.
	/// </summary>
	public string Iin { get; set; }

	/// <summary>
	///     ФИО.
	/// </summary>
	public string FullName { get; set; }

	/// <summary>
	///     Тип учетной записи.
	/// </summary>
	public LoginRole Usertype { get; set; }

	/// <summary>
	///     Выбранный язык по умолчанию.
	/// </summary>
	public Language Language { get; set; }

	/// <summary>
	///     Логин пользователя, указанный пользователем.
	/// </summary>
	public string Username { get; set; }

	/// <summary>
	///     Признак, что учетная запись заблокирована.
	/// </summary>
	public bool Blocked { get; set; }

	/// <summary>
	///     Причина блокировки.
	/// </summary>
	public string BlockedReason { get; set; }

	/// <summary>
	///     Дата удаления.
	/// </summary>
	public DateTime? DeletedAt { get; set; }

	/// <summary>
	///     Признак, что учетная запись удалена.
	/// </summary>
	public bool Deleted { get; set; }
}