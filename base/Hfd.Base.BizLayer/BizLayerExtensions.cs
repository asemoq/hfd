using System.Reflection;
using MediatR.Pipeline;
using Hfd.Base.BizLayer.Internals;

namespace Hfd.Base.BizLayer;

public static class BizLayerExtensions
{
	/// <summary>
	///     Регистрация сервисов репозитория.
	/// </summary>
	/// <param name="assemblies">Сборки, в которых ищем поставщиков данных.</param>
	/// <returns>Измененные сервисы приложения.</returns>
	public static IServiceCollection AddHfdBaseBizLayerServices(this IServiceCollection services,
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
			.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssemblies(assemblies);
				cfg.AddOpenRequestPreProcessor(typeof(ValidationPreProcessor<>));
			});
	}
}