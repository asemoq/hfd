using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Model;

namespace Hfd.Base.Contract.Services;

/// <summary>
///     User service.
/// </summary>
public interface IUserLoginDbService : IDbService
{
	/// <summary>
	///     Получить список пользователей.
	/// </summary>
	/// <param name="filter">Параметры запроса.</param>
	/// <returns>Список пользователей.</returns>
	Task<IPageList<IUserFull>> GetAll(UserFilter filter);

	/// <summary>
	///     Добавить нового пользователя.
	/// </summary>
	/// <param name="type">Тип учетной записи.</param>
	/// <param name="user">Данные пользователя.</param>
	/// <param name="password">Образ пароля для хранения в базе.</param>
	/// <returns>Созданный пользователь.</returns>
	Task<IUserFull> Create(LoginRole type, UserInfo user, string password);

	/// <summary>
	///     Найти пользователя по идентификатору.
	/// </summary>
	/// <param name="id">Идентификатор пользователя.</param>
	/// <param name="uid">Уникальный идентификатор пользователя.</param>
	/// <returns>Данные пользователя.</returns>
	Task<IUserFull> GetOne(long id);

	/// <summary>
	///     Найти пользователя по логину.
	/// </summary>
	/// <param name="username">Логин пользователя.</param>
	/// <returns>Данные пользователя.</returns>
	Task<IUserFull> GetOne(string username);

	/// <summary>
	///     Изменить пользователя.
	/// </summary>
	/// <param name="id">Идентификатор пользователя.</param>
	/// <param name="user">Данные пользователя.</param>
	/// <param name="password">Образ пароля для хранения в базе.</param>
	/// <returns>Обновлённый пользователь.</returns>
	Task<IUserFull> Update(long id, UserInfo user, string password);

	/// <summary>
	///     Изменить пользователя.
	/// </summary>
	/// <param name="id">Идентификатор пользователя.</param>
	/// <param name="password">Образ пароля для хранения в базе.</param>
	/// <returns>Асинхронный результат.</returns>
	Task ChangePassword(long id, string password);

	/// <summary>
	///     Изменить блокировку учетной записи.
	/// </summary>
	/// <param name="id">Идентификатор пользователя.</param>
	/// <param name="blocked">Заблокировать.</param>
	/// <param name="blockedReason">Причина блокировки.</param>
	Task<IUserFull> Block(long id, bool blocked, string blockedReason = default);

	/// <summary>
	///     Найти пользователя по идентификатору.
	/// </summary>
	/// <param name="id">Идентификатор пользователя.</param>
	/// <returns>Данные пользователя.</returns>
	Task Delete(long id);
}