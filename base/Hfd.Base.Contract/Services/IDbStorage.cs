namespace Hfd.Base.Contract.Services;

/// <summary>
///     Репозиторий приложения.
/// </summary>
public interface IDbStorage
{
	/// <summary>
	///     Получить поставщика данных.
	/// </summary>
	/// <typeparam name="TDbService">Тип поставщика данных.</typeparam>
	/// <returns>Поставщик данных.</returns>
	TDbService Get<TDbService>()
		where TDbService : IDbService;

	/// <summary>
	///     Начать транзакцию.
	/// </summary>
	void BeginTransaction();

	/// <summary>
	///     Подтвердить транзакцию.
	/// </summary>
	void CommitTransaction();

	/// <summary>
	///     Откатить транзакцию.
	/// </summary>
	void RollbackTransaction();
}