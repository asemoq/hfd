using System.Reflection;
using Hfd.Base.BizLayer.Internals;
using Hfd.Base.BizLayer.Services;
using Hfd.Base.Contract.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Hfd.Base.BizLayer;

public static class BizLayerExtensions
{
	/// <summary>
	///     Регистрация сервисов репозитория.
	/// </summary>
	/// <param name="configuration"></param>
	/// <param name="assemblies">Сборки, в которых ищем поставщиков данных.</param>
	/// <returns>Измененные сервисы приложения.</returns>
	public static IServiceCollection AddHfdBaseBizLayerServices(this IServiceCollection services,
		IConfiguration configuration, Assembly[] assemblies)
	{
		services.Configure<ProviderOptions>(configuration.GetSection("providerOptions"))
			.AddTransient<IProviderOptions>(p => p.GetRequiredService<IOptions<ProviderOptions>>().Value);
		
		services
			.AddHttpClient().AddTransient(typeof(IClientFactory<>), typeof(ClientFactory<>));

		services
			.AddHttpClient(ClientFactory.UserClientName, (_, _) => { });

		services
			.AddHttpClient(ClientFactory.ServiceClientName, (_, _) => { });

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