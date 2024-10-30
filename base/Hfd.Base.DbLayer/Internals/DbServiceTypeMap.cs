using System.Collections.Concurrent;
using System.Reflection;
using Hfd.Base.DbLayer.Services;

namespace Hfd.Base.DbLayer.Internals;

internal interface IDbServiceTypeMap
{
	/// <summary>
	///     Возвращает тип, реализующий интерфейс.
	/// </summary>
	/// <param name="serviceType">Интерфейс поставщика данных.</param>
	/// <returns>Тип, реализующий интерфейс.</returns>
	Type Get(Type serviceType);
}

/// <summary>
///     Карта соответствия интерфейсов и реализаций поставщиков данных.
/// </summary>
internal sealed class DbServiceTypeMap<TDbStorage, TDataModel> : IDbServiceTypeMap
	where TDbStorage : DbStorage<TDataModel>
	where TDataModel : class, IDataModel
{
	/// <summary>
	///     Типы поставщиков данных.
	/// </summary>
	private readonly List<Type> _implementations;

	/// <summary>
	///     Кэш найденных реализаций.
	/// </summary>
	private readonly ConcurrentDictionary<Type, Type> _typeMap;

	/// <summary>
	///     Инициализирует новый экземпляр класса <see cref="DbServiceTypeMap{TDbStorage, TDbContext}" />.
	/// </summary>
	/// <param name="assemblies">Сборки, в которых ищем поставщиков данных.</param>
	public DbServiceTypeMap(Assembly[] assemblies)
	{
		_implementations = assemblies
			.SelectMany(a => a
				.GetExportedTypes()
				.Where(t => !t.IsAbstract && typeof(DbService<TDbStorage, TDataModel>).IsAssignableFrom(t)))
			.ToList();

		_typeMap = new ConcurrentDictionary<Type, Type>();
	}

	/// <summary>
	///     Создает поставщика данных.
	/// </summary>
	/// <param name="serviceType">Тип поставщика данных.</param>
	/// <returns>Поставщика данных.</returns>
	public Type Get(Type serviceType)
	{
		return _typeMap.GetOrAdd(serviceType, s => _implementations.Single(t => s.IsAssignableFrom(t)));
	}
}