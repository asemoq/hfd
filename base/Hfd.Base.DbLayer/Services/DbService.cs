using Hfd.Base.Contract.Services;
using Microsoft.Extensions.Logging;

namespace Hfd.Base.DbLayer.Services;

/// <summary>
///     Базовый класс для реализации сервисов данных.
/// </summary>
/// <typeparam name="TDbStorage">Тип репозитория данных.</typeparam>
/// <typeparam name="TDataModel">Тип модели данных.</typeparam>
public abstract class DbService<TDbStorage, TDataModel> : IDbService
	where TDbStorage : DbStorage<TDataModel>
	where TDataModel : class, IDataModel
{
	/// <summary>
	///     Initializes a new instance of the <see cref="DbService{TDbStorage, TDbContext}" /> class.
	/// </summary>
	/// <param name="storage">Репозиторий приложения.</param>
	/// <param name="log">Журнал событий.</param>
	public DbService(TDbStorage storage, ILogger log)
	{
		Storage = storage;
		Log = log;
	}

	/// <inheritdoc cref="IDbService.Storage" />
	protected TDbStorage Storage { get; }

	/// <summary>
	///     Журнал приложения.
	/// </summary>
	protected ILogger Log { get; }

	/// <inheritdoc />
	IDbStorage IDbService.Storage => Storage;

	/// <summary>
	///     Обертка метода обращения к репозиторию данных.
	/// </summary>
	/// <typeparam name="T">Тип результата метода.</typeparam>
	/// <param name="method">Метод обращения к репозиторию данных.</param>
	/// <returns>Результат метода.</returns>
	protected async Task<T> Execute<T>(Func<TDataModel, Task<T>> method)
	{
		try
		{
			return await method(Storage.EnsureConnected());
		}
		catch (Exception innerEx)
		{
			throw Storage.HandleError(innerEx);
		}
	}

	/// <summary>
	///     Обертка метода обращения к репозиторию данных.
	/// </summary>
	/// <param name="method">Метод обращения к репозиторию данных.</param>
	/// <returns>Результат метода.</returns>
	protected async Task Execute(Func<TDataModel, Task> method)
	{
		try
		{
			await method(Storage.EnsureConnected());
		}
		catch (Exception innerEx)
		{
			throw Storage.HandleError(innerEx);
		}
	}

	/// <summary>
	///     Обертка метода обращения к репозиторию данных.
	/// </summary>
	/// <typeparam name="T">Тип результата метода.</typeparam>
	/// <param name="method">Метод обращения к репозиторию данных.</param>
	/// <returns>Результат метода.</returns>
	protected T Execute<T>(Func<TDataModel, T> method)
	{
		try
		{
			return method(Storage.EnsureConnected());
		}
		catch (Exception innerEx)
		{
			throw Storage.HandleError(innerEx);
		}
	}

	/// <summary>
	///     Обертка метода обращения к репозиторию данных.
	/// </summary>
	/// <param name="method">Метод обращения к репозиторию данных.</param>
	protected void Execute(Action<TDataModel> method)
	{
		try
		{
			method(Storage.EnsureConnected());
		}
		catch (Exception innerEx)
		{
			throw Storage.HandleError(innerEx);
		}
	}
}