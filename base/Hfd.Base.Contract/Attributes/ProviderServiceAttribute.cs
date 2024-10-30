using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Base.Contract.Attributes;

/// <summary>
///     Атрибут для классов, доступных через ProviderServiceFactory.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
internal sealed class ProviderServiceAttribute : Attribute
{
	/// <summary>
	///     Инициализирует новый экземпляр класса <see cref="ProviderServiceAttribute" />.
	/// </summary>
	/// <param name="type">Тип провайдера сервиса.</param>
	/// <param name="lifetime">Lifetime of the service.</param>
	public ProviderServiceAttribute(string type, ServiceLifetime lifetime = ServiceLifetime.Transient)
		: this(lifetime, type)
	{
	}

	/// <summary>
	///     Инициализирует новый экземпляр класса <see cref="ProviderServiceAttribute" />.
	/// </summary>
	/// <param name="lifetime">Lifetime of the service.</param>
	/// <param name="providerTypes">Типы провайдеров сервиса.</param>
	private ProviderServiceAttribute(ServiceLifetime lifetime, params string[] providerTypes)
	{
		Lifetime = lifetime;
		ProviderTypes = providerTypes;
	}

	/// <summary>
	///     Lifetime of the service.
	/// </summary>
	public ServiceLifetime Lifetime { get; }

	/// <summary>
	///     Типы провайдеров сервиса.
	/// </summary>
	public string[] ProviderTypes { get; }

	/// <summary>
	///     Создает экземпляр типа с указанным name.
	/// </summary>
	/// <param name="provider">Сервисы приложения.</param>
	/// <param name="type">Тип создаваемого объекта.</param>
	/// <param name="name">Имя сервиса.</param>
	/// <returns>Экземпляр типа.</returns>
	public object Create(IServiceProvider provider, Type? type, string name)
	{
		if (type != null) return provider.GetRequiredService(type);
		throw new InvalidOperationException();
	}
}