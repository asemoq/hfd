using Hfd.Base.Contract.Services;

namespace Hfd.Base.DbLayer.Services;

/// <summary>
///     Интерфейс фабрики сервисов данных.
/// </summary>
internal interface IDbServiceFactory
{
	/// <summary>
	///     Метод создания сервиса данных.
	/// </summary>
	/// <typeparam name="TDbService">Тип сервиса данных.</typeparam>
	/// <returns>Инстанс сервиса данных.</returns>
	public TDbService Create<TDbService>()
		where TDbService : IDbService;
}

/// <summary>
///     Фабрика сервисов данных.
/// </summary>
internal sealed class DbServiceFactory : IDbServiceFactory
{
	/// <summary>
	///     Фабрика контекста данных.
	/// </summary>
	private readonly IDbStorage _storage;

	/// <summary>
	///     Инициализирует новый экземпляр класса <see cref="DbServiceFactory" />.
	/// </summary>
	/// <param name="storage">Фабрика контекста данных.</param>
	public DbServiceFactory(IDbStorage storage)
	{
		_storage = storage;
	}

	/// <inheritdoc />
	public TDbService Create<TDbService>()
		where TDbService : IDbService
	{
		return _storage.Get<TDbService>();
	}
}