using Hfd.Base.Contract.Dto;
using RestEase;

namespace Hfd.Base.Contract.Services;

/// <summary>
/// Клиент любой подсистемы
/// </summary>
public interface IClient : IDisposable
{
	/// <summary>
	/// Получает или задает токен доступа.
	/// </summary>
	[Header("Authorization")]
	string Authorization { get; set; }

	/// <summary>
	/// Получает или задает токен доступа.
	/// </summary>
	[Header("X-Api-User")]
	string UserFull { get; set; }

	/// <summary>
	/// Реальный IP адрес клиента.
	/// </summary>
	[Header("X-Forwarded-For")]
	string ForwardedFor { get; set; }

	/// <summary>
	/// Возвращает "pong".
	/// </summary>
	/// <param name="ping">Произвольная строка.</param>
	/// <param name="token">Токен отмены.</param>
	/// <returns>pong.</returns>
	[Get("api/subsystem/ping")]
	Task<string> Ping(string? ping = null, CancellationToken token = default);

	/// <summary>
	/// Получить учетную запись текущего пользователя.
	/// </summary>
	/// <param name="token">Токен отмены.</param>
	/// <returns>Список подсистем.</returns>
	[Get("api/subsystem/userSelf")]
	Task<UserFull?> GetUserSelf(CancellationToken token = default);
}