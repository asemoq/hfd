using System.Reflection;
using Hfd.Base.Contract.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Base.Contract.Services;

/// <summary>
///     Интерфейс фабрики сервисов, доступных по имени.
/// </summary>
/// <remarks>
///     Текущая реализация требует, чтобы имена сервисов были уникальны в рамках solution.
/// </remarks>
public interface INamedServiceFactory
{
	/// <summary>
	///     Get service by name, please, use typed extensions.
	/// </summary>
	/// <param name="name">Name of the service.</param>
	/// <returns>A service object.</returns>
	object GetRequiredService(string name);

	/// <summary>
	///     Get service by name or null if not exists.
	/// </summary>
	/// <param name="name">Name of the service.</param>
	/// <returns>A service object.</returns>
	object GetService(string name);

	/// <summary>
	///     Служебный метод: узнать тип сервиса по имени.
	/// </summary>
	/// <param name="name">Name of the implementation service.</param>
	/// <returns>Service type.</returns>
	Type GetServiceType(string name);

	/// <summary>
	///     Служебный метод: узнать описание сервиса по имени.
	/// </summary>
	/// <typeparam name="T">The type of service info.</typeparam>
	/// <param name="name">Name of the service.</param>
	/// <returns>Service type.</returns>
	T GetServiceInfo<T>(string name)
		where T : NamedServiceAttribute;
}

/// <summary>
///     Регистрирует фабрику поименованных сервисов.
/// </summary>
public static class NamedServiceExtensions
{
	/// <summary>
	///     Get named service of type T.
	/// </summary>
	/// <typeparam name="T">The type of service object to get.</typeparam>
	/// <param name="serviceProvider">Поставщик сервисов.</param>
	/// <param name="name">Name of the implementation service.</param>
	/// <returns>A service object of type T.</returns>
	public static T GetNamedService<T>(this IServiceProvider serviceProvider, string name)
	{
		return serviceProvider.GetRequiredService<INamedServiceFactory>().GetRequiredService<T>(name);
	}

	/// <summary>
	///     Get service of type T.
	/// </summary>
	/// <typeparam name="T">The type of service object to get.</typeparam>
	/// <param name="factory">Фабрика поименованных сервисов.</param>
	/// <param name="name">Name of the implementation service.</param>
	/// <returns>A service object of type T.</returns>
	public static T GetRequiredService<T>(this INamedServiceFactory factory, string name)
	{
		return (T)factory.GetRequiredService(name);
	}

	/// <summary>
	///     Get service of type T.
	/// </summary>
	/// <typeparam name="T">The type of service object to get.</typeparam>
	/// <param name="factory">Фабрика поименованных сервисов.</param>
	/// <param name="name">Name of the implementation service.</param>
	/// <returns>A service object of type T.</returns>
	public static T GetService<T>(this INamedServiceFactory factory, string name)
	{
		var t = factory.GetService(name);
		return t != null ? (T)t : default;
	}

	/// <summary>
	///     Метод регистрации фабрики поименованных сервисов.
	/// </summary>
	/// <param name="services">Коллекция сервисов приложения.</param>
	/// <param name="assemblies">Сборки приложения.</param>
	/// <returns>Измененная коллекция сервисов приложения.</returns>
	internal static IServiceCollection AddNamedServices(this IServiceCollection services, Assembly[] assemblies)
	{
		return services
			.AddSingleton(new TypeMap(assemblies
				.SelectMany(a => a.GetExportedTypes().Where(t => t.IsDefined(typeof(NamedServiceAttribute), true)))
				.Select(t => t.AddSelf(services))))
			.AddTransient<INamedServiceFactory, Factory>();
	}

	/// <summary>
	///     Регистрирует сервис в соответствии с данными аттрибута.
	/// </summary>
	/// <param name="t">Тип сервиса.</param>
	/// <param name="sc">Коллекция сервисов.</param>
	/// <returns>Пара (NamedServiceAttribute, реализация сервиса).</returns>
	private static Tuple<NamedServiceAttribute, Type> AddSelf(this Type t, IServiceCollection sc)
	{
		var a = t.GetCustomAttribute<NamedServiceAttribute>(true);
		sc.Add(ServiceDescriptor.Describe(t, t, a.Lifetime));
		return Tuple.Create(a, t);
	}

	#region [ Вложенный класс TypeMap ]

	/// <summary>
	///     Класс для хранения словаря [имя сервиса] = (NamedServiceAttribute, реализация сервиса).
	/// </summary>
	private class TypeMap
	{
		/// <summary>
		///     Словарь [имя сервиса] = (NamedServiceAttribute, реализация сервиса).
		/// </summary>
		private readonly Dictionary<string, Tuple<NamedServiceAttribute, Type>> _services;

		/// <summary>
		///     Инициализирует новый экземпляр класса <see cref="TypeMap" />.
		/// </summary>
		/// <param name="tuples">Пары (NamedServiceAttribute, реализация сервиса) для хранения в словаре.</param>
		public TypeMap(IEnumerable<Tuple<NamedServiceAttribute, Type>> tuples)
		{
			_services = new Dictionary<string, Tuple<NamedServiceAttribute, Type>>();
			foreach (var tuple in tuples)
				Array.ForEach(tuple.Item1.Names, name => _services.Add(name, tuple));
		}

		/// <summary>
		///     Возвращает реализацию сервиса по имени.
		/// </summary>
		/// <param name="name">Имя сервиса.</param>
		/// <returns>Тип сервиса, соответствующий имени сервиса.</returns>
		public Type this[string name] => _services[name].Item2;

		/// <summary>
		///     Возвращает реализацию сервиса по имени.
		/// </summary>
		/// <param name="name">Имя сервиса.</param>
		/// <param name="type">Найденный тип, реализующий сервис.</param>
		/// <returns>Признак что тип найден.</returns>
		public bool TryGet(string name, out Type type)
		{
			if (_services.TryGetValue(name, out var t))
			{
				type = t.Item2;
				return true;
			}

			type = null;
			return false;
		}

		/// <summary>
		///     Возвращает описание сервиса по имени.
		/// </summary>
		/// <param name="name">Имя сервиса.</param>
		/// <returns>Тип сервиса, соответствующий имени сервиса.</returns>
		public NamedServiceAttribute GetInfo(string name)
		{
			return _services[name].Item1;
		}
	}

	#endregion

	#region [ Вложенный класс Factory ]

	/// <inheritdoc />
	private class Factory : INamedServiceFactory
	{
		/// <summary>
		///     Словарь поиска типа сервиса по имени.
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
		public object GetRequiredService(string name)
		{
			return _map.GetInfo(name).Create(_provider, _map[name], name);
		}

		/// <inheritdoc />
		public object GetService(string name)
		{
			return _map.TryGet(name, out var type) ? _map.GetInfo(name).Create(_provider, type, name) : null;
		}

		/// <inheritdoc />
		public Type GetServiceType(string name)
		{
			return _map[name];
		}

		/// <inheritdoc />
		public T GetServiceInfo<T>(string name)
			where T : NamedServiceAttribute
		{
			return (T)_map.GetInfo(name);
		}
	}

	#endregion
}