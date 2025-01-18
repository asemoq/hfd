namespace Hfd.Base.Contract.Dto;

/// <summary>
///     Login response.
/// </summary>
public class LoginFull
{
	/// <summary>
	///     Роль пользователя
	/// </summary>
	public LoginRole Usertype { get; set; }

	/// <summary>
	///     Login пользователя
	/// </summary>
	public string Username { get; set; }

	/// <summary>
	///     JWT token.
	/// </summary>
	public string Token { get; set; }
}