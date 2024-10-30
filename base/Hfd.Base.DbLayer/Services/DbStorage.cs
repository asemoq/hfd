using System.Collections.Concurrent;
using System.Data;
using Hfd.Base.Contract.Internals;
using Hfd.Base.Contract.Services;
using Hfd.Base.DbLayer.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hfd.Base.DbLayer.Services;

/// <summary>
///     Базовая реализация репозитория приложения.
/// </summary>
/// <typeparam name="TDataModel">Тип модели данных.</typeparam>
public abstract class DbStorage<TDataModel> : IDbStorage, IDisposable
	where TDataModel : class, IDataModel
{
	/// <summary>
	///     Фабрика модели данных.
	/// </summary>
	private readonly Func<TDataModel> _modelFactory;

	/// <summary>
	///     Фабрика поставщиков данных.
	/// </summary>
	private readonly Func<Type, object> _serviceFactory;

	/// <summary>
	///     Коллекция созданных поставщиков.
	/// </summary>
	private readonly ConcurrentDictionary<Type, object> _services = new();

	/// <summary>
	///     Контекст подключения.
	/// </summary>
	private IDisposable _connection;

	/// <summary>
	///     Модель данных.
	/// </summary>
	private TDataModel _dataModel;

	/// <summary>
	///     Шаблон IDisposable.
	/// </summary>
	private bool _disposed;

	/// <summary>
	///     Текущая транзакция.
	/// </summary>
	private IDisposable _transaction;

	/// <summary>
	///     Признак отката транзакции.
	/// </summary>
	private bool _transactionCanceled;

	/// <summary>
	///     Вложенность транзакции.
	/// </summary>
	private int _transactionLevel;

	/// <summary>
	///     Инициализирует новый экземпляр класса <see cref="DbStorage{TDbContext}" />.
	/// </summary>
	/// <param name="sp">Поставщик сервисов.</param>
	/// <param name="log">Журнал событий.</param>
	public DbStorage(IServiceProvider sp, ILogger log)
	{
		_serviceFactory = serviceType =>
		{
			var sellingType = sp.GetRequiredService<IDbServiceTypeMap>().Get(serviceType);
			return _services.GetOrAdd(sellingType, ActivatorUtilities.CreateInstance(sp, sellingType));
		};
		_modelFactory = () => { return sp.GetRequiredService<IDataModelFactory<TDataModel>>().Create(); };
		Log = log;
	}

	/// <summary>
	///     Журнал событий.
	/// </summary>
	protected ILogger Log { get; }

	/// <inheritdoc />
	public TDbService Get<TDbService>()
		where TDbService : IDbService
	{
		return (TDbService)_serviceFactory(typeof(TDbService));
	}

	/// <inheritdoc />
	public void BeginTransaction()
	{
		if (_transactionLevel == 0)
		{
			_transaction = BeginTransaction(EnsureConnected());
			_transactionCanceled = false;
		}

		Log.LogTrace($"Начата транзакция уровня {_transactionLevel}.");
		_transactionLevel++;
	}

	/// <inheritdoc />
	public void CommitTransaction()
	{
		if (_transactionCanceled)
			throw new DataException("Отмененная транзакция не может быть подтверждена.");

		if (_transactionLevel == 1)
		{
			CommitTransaction(_dataModel);
			_transaction?.Dispose();
		}

		if (--_transactionLevel < 0)
			throw new DataException($"Невозможно подтвердить транзакцию уровня {_transactionLevel}.");

		Log.LogTrace($"Подтверждена транзакция уровня {_transactionLevel}.");
	}

	/// <inheritdoc />
	public void RollbackTransaction()
	{
		_transactionCanceled = true;

		if (_transactionLevel == 1)
		{
			RollbackTransaction(_dataModel);
			_transaction?.Dispose();
		}

		if (--_transactionLevel < 0)
			throw new DataException($"Невозможно отменить транзакцию уровня {_transactionLevel}.");

		Log.LogTrace($"Отменена транзакция уровня {_transactionLevel}.");
	}

	/// <inheritdoc />
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	///     Уничтожает экземпляр класса <see cref="DbStorage{TDataModel}" />.
	/// </summary>
	~DbStorage()
	{
		Dispose(false);
	}

	/// <summary>
	///     Открывает соединение к базе данных.
	/// </summary>
	/// <returns>Модель данных.</returns>
	internal TDataModel EnsureConnected()
	{
		if (_disposed)
			throw new DataException("Недопустимая операция", new ObjectDisposedException(GetType().Name));

		if (_connection == null)
			_connection = new AnonymousDisposable(Close);

		return _dataModel ??= _modelFactory();
	}

	/// <summary>
	///     Преобразует ошибку времени исполнения в унифицированное представление.
	/// </summary>
	/// <param name="innerEx">Ошибку времени исполнения.</param>
	/// <returns>Унифицированное представление ошибки.</returns>
	internal DataException HandleError(Exception innerEx)
	{
		Log.LogError(innerEx, $"Ошибка при обращении к репозиторию данных: {innerEx.Message}");

		return OverrideError(innerEx) ??
				new DataException("Ошибка при обращении к репозиторию данных.", innerEx);
	}

	/// <summary>
	///     Шаблон IDisposable: освобождает управляемые и неуправляемые ресурсы.
	/// </summary>
	/// <param name="disposing">Признак освобождения управляемых ресурсов.</param>
	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
			return;

		if (disposing)
			_connection?.Dispose();

		_disposed = true;
	}

	/// <summary>
	///     Создает новую транзакцию уровня 1.
	/// </summary>
	/// <param name="model">Модаль данных.</param>
	/// <returns>Объект, который будет освобожден после завершения транзакции уровня 0.</returns>
	protected abstract IDisposable BeginTransaction(TDataModel model);

	/// <summary>
	///     Принимает транзакцию уровня 0.
	/// </summary>
	/// <param name="model">Модаль данных.</param>
	protected abstract void CommitTransaction(TDataModel model);

	/// <summary>
	///     Откатывает транзакцию уровня 0.
	/// </summary>
	/// <param name="model">Модаль данных.</param>
	protected abstract void RollbackTransaction(TDataModel model);

	/// <summary>
	///     Преобразует ошибку времени исполнения в унифицированное представление.
	/// </summary>
	/// <param name="ex">Ошибку времени исполнения.</param>
	/// <returns>Унифицированное представление ошибки.</returns>
	protected virtual DataException OverrideError(Exception ex)
	{
		return ex as DataException;
	}

	/// <summary>
	///     Закрывает соединение и отменяет активную транзакцию, если она есть.
	/// </summary>
	private void Close()
	{
		if (_transactionLevel != 0)
		{
			RollbackTransaction(_dataModel);
			using (_transaction)
			{
				_transaction = null;
				Log.LogCritical("Откат транзакции, вышедшей за область действия соединения.");
			}

			_transactionCanceled = false;
			_transactionLevel = 0;
		}

		using (_dataModel)
		{
			_dataModel = null;
		}

		_connection = null;
		Log.LogDebug("Disposed data model.");
	}
}