using System.Reflection;

namespace Hfd.BizLayer;

public static class BizLayerExtensions
{
	/// <summary>
	///     Регистрация сервисов.
	/// </summary>
	/// <param name="services"></param>
	/// <param name="assemblies">Сборки, в которых ищем сервисы.</param>
	/// <returns>Сервисы приложения.</returns>
	public static IServiceCollection AddHfdBizLayerServices(this IServiceCollection services,
		Assembly[] assemblies)
	{
		services
			.AddHttpClient();

		return services
			.Scan(scan => scan
				.FromAssemblies(assemblies)
				.AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
				.AsSelfWithInterfaces()
				.WithTransientLifetime())
			.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
	}
}