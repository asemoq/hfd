using System.Reflection;
using Hfd.Base.Contract.Attributes;
using Hfd.Base.Contract.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Base.Contract.Services;

/// <summary>
///     Интерфейс фабрики сервисов, доступных по типу провайдера.
/// </summary>
public interface IProviderServiceFactory
{
	/// <summary>
	///     Get service by provider type, please, use typed extensions.
	/// </summary>
	/// <param name="type">Provider type</param>
	/// <returns>A service object.</returns>
	object? GetRequiredService(ProviderType type);

	/// <summary>
	///     Get service by name or null if not exists.
	/// </summary>
	/// <param name="type">Provider type.</param>
	/// <returns>A service object.</returns>
	object? GetService(ProviderType type);
}

/// <summary>
///     Регистрирует фабрику провайдеров.
/// </summary>
public static class ProviderServiceExtensions
{
	/// <summary>
	///     Get named service of type T.
	/// </summary>
	/// <typeparam name="T">The type of service object to get.</typeparam>
	/// <param name="serviceProvider">Поставщик сервисов.</param>
	/// <param name="type">Provider type.</param>
	/// <returns>A service object of type T.</returns>
	public static T? GetProviderService<T>(this IServiceProvider serviceProvider, ProviderType type)
	{
		return serviceProvider.GetRequiredService<IProviderServiceFactory>().GetRequiredService<T>(type);
	}

	/// <summary>
	///     Get service of type T.
	/// </summary>
	/// <typeparam name="T">The type of service object to get.</typeparam>
	/// <param name="factory">Фабрика поименованных сервисов.</param>
	/// <param name="type">Provider type.</param>
	/// <returns>A service object of type T.</returns>
	private static T? GetRequiredService<T>(this IProviderServiceFactory factory, ProviderType type)
	{
		var t = factory.GetRequiredService(type);
		return t != null ? (T)t : default;
	}

	/// <summary>
	///     Get service of type T.
	/// </summary>
	/// <typeparam name="T">The type of service object to get.</typeparam>
	/// <param name="factory">Фабрика провайдеров.</param>
	/// <param name="type">Provider type.</param>
	/// <returns>A service object of type T.</returns>
	public static T? GetService<T>(this IProviderServiceFactory factory, ProviderType type)
	{
		var t = factory.GetService(type);
		return t != null ? (T)t : default;
	}

	/// <summary>
	///     Метод регистрации фабрики поименованных сервисов.
	/// </summary>
	/// <param name="services">Коллекция сервисов приложения.</param>
	/// <param name="assemblies">Сборки приложения.</param>
	/// <returns>Измененная коллекция сервисов приложения.</returns>
	public static IServiceCollection
		AddProviderServices(this IServiceCollection services, IEnumerable<Assembly> assemblies)
	{
		return services
			.AddSingleton(new TypeMap(assemblies
				.SelectMany(a =>
					a.GetExportedTypes().Where(t => t.IsDefined(typeof(ProviderServiceAttribute), true)))
				.Select(t => t.AddSelf(services))))
			.AddTransient<IProviderServiceFactory, Factory>();
	}

	/// <summary>
	///     Регистрирует сервис в соответствии с данными аттрибута.
	/// </summary>
	/// <param name="t">Тип сервиса.</param>
	/// <param name="sc">Коллекция сервисов.</param>
	/// <returns>Пара (MessageServiceAttribute, реализация сервиса).</returns>
	private static Tuple<ProviderServiceAttribute, Type> AddSelf(this Type t, IServiceCollection sc)
	{
		var a = t.GetCustomAttribute<ProviderServiceAttribute>(true);
		if (a == null) throw new InvalidOperationException();
		sc.Add(ServiceDescriptor.Describe(t, t, a.Lifetime));
		return Tuple.Create(a, t);
	}

	#region [ Вложенный класс TypeMap ]

	/// <summary>
	///     Класс для хранения словаря [тип провайдера] = (ProviderServiceAttribute, реализация сервиса).
	/// </summary>
	private sealed class TypeMap
	{
		/// <summary>
		///     Словарь [тип провайдера] = (ProviderServiceAttribute, реализация сервиса).
		/// </summary>
		private readonly Dictionary<string, Tuple<ProviderServiceAttribute, Type>> _services;

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="TypeMap" />.
		/// </summary>
		/// <param name="tuples">Пары (ProviderServiceAttribute, реализация сервиса) для хранения в словаре.</param>
		public TypeMap(IEnumerable<Tuple<ProviderServiceAttribute, Type>> tuples)
		{
			_services = new Dictionary<string, Tuple<ProviderServiceAttribute, Type>>();
			foreach (var tuple in tuples)
				Array.ForEach(tuple.Item1.ProviderTypes, type => _services.Add(type, tuple));
		}

		/// <summary>
		///     Возвращает реализацию сервиса по типу провайдера.
		/// </summary>
		/// <param name="type">Тип провайдера.</param>
		/// <returns>Тип сервиса, соответствующий имени сервиса.</returns>
		public Type this[string type] => _services[type].Item2;

		/// <summary>
		///     Возвращает реализацию сервиса по типу провайдера.
		/// </summary>
		/// <param name="providerType">Тип провайдера.</param>
		/// <param name="type">Найденный тип, реализующий сервис.</param>
		/// <returns>Признак, что тип найден.</returns>
		public bool TryGet(string providerType, out Type? type)
		{
			if (_services.TryGetValue(providerType, out var t))
			{
				type = t.Item2;
				return true;
			}

			type = null;
			return false;
		}

		/// <summary>
		///     Возвращает описание сервиса по типу провайдера.
		/// </summary>
		/// <param name="type">Тип провайдера.</param>
		/// <returns>Тип сервиса, соответствующий типу провайдера.</returns>
		public ProviderServiceAttribute GetInfo(string type)
		{
			return _services[type].Item1;
		}
	}

	#endregion

	#region [ Вложенный класс Factory ]

	/// <inheritdoc />
	private sealed class Factory : IProviderServiceFactory
	{
		/// <summary>
		///     Словарь поиска типа сервиса по типу провайдера.
		/// </summary>
		private readonly TypeMap _map;

		/// <summary>
		///     Сервисы приложения.
		/// </summary>
		private readonly IServiceProvider _provider;

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="Factory" />.
		/// </summary>
		/// <param name="provider">Сервисы приложения.</param>
		public Factory(IServiceProvider provider)
		{
			_map = (_provider = provider).GetRequiredService<TypeMap>();
		}

		/// <inheritdoc />
		public object GetRequiredService(ProviderType type)
		{
			return _map.GetInfo(type.ToString()).Create(_provider, _map[type.ToString()], type.ToString());
		}

		/// <inheritdoc />
		public object? GetService(ProviderType providerType)
		{
			return _map.TryGet(providerType.ToString(), out var type)
				? _map.GetInfo(providerType.ToString()).Create(_provider, type, providerType.ToString())
				: null;
		}
	}

	#endregion
}