using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Base.Contract.Attributes;

/// <summary>
///     Атрибут для классов, доступных через NamedServiceFactory.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class NamedServiceAttribute : Attribute
{
	/// <summary>
	///     Инициализирует новый экземпляр класса <see cref="NamedServiceAttribute" />.
	/// </summary>
	/// <param name="name">Название сервиса.</param>
	/// <param name="lifetime">Lifetime of the service..</param>
	public NamedServiceAttribute(string name, ServiceLifetime lifetime = ServiceLifetime.Transient)
		: this(lifetime, name)
	{
	}

	/// <summary>
	///     Инициализирует новый экземпляр класса <see cref="NamedServiceAttribute" />.
	/// </summary>
	/// <param name="lifetime">Lifetime of the service..</param>
	/// <param name="names">Названия сервиса.</param>
	public NamedServiceAttribute(ServiceLifetime lifetime, params string[] names)
	{
		Lifetime = lifetime;
		Names = names;
	}

	/// <summary>
	///     Lifetime of the service.
	/// </summary>
	public ServiceLifetime Lifetime { get; }

	/// <summary>
	///     Названия сервиса.
	/// </summary>
	public string[] Names { get; }

	/// <summary>
	///     Создает экземпляр типа с указанным name.
	/// </summary>
	/// <param name="provider">Сервисы приложения.</param>
	/// <param name="type">Тип создаваемого объекта.</param>
	/// <param name="name">Имя сервиса.</param>
	/// <returns>Экземпляр типа.</returns>
	public virtual object Create(IServiceProvider provider, Type type, string name)
	{
		return provider.GetRequiredService(type);
	}
}