using Hfd.Base.Contract.Model;

namespace Hfd.Base.Contract.Services;

/// <summary>
///     Сервис управления пользователями провайдеров.
/// </summary>
public interface IProviderUserService : IDbService
{
	/// <summary>
	///     Сохранить пользователя в БД.
	/// </summary>
	Task<IProviderUserFull> Add(long patientId, bool? whatsapp, bool? telegram);

	/// <summary>
	///     Изменить пользователя в БД.
	/// </summary>
	Task<IProviderUserFull> Edit(long userId, long patientId, bool? whatsapp, bool? telegram);

	/// <summary>
	///     Получить пользователя из БД по идентификатору сотрудника.
	/// </summary>
	Task<IProviderUserFull> GetByPatientId(long patientId);

	/// <summary>
	///     Изменить статус подписки на рассылку по telegram.
	/// </summary>
	Task<IProviderUserFull> SetTelegramSubscribedStatus(long userId, bool subscribed);

	/// <summary>
	///     Удалить пользователя.
	/// </summary>
	Task<int> Delete(long userId);
}