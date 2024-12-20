using System.Reflection;
using Hfd.Base.BizLayer;

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
		return services
			.AddHfdBaseBizLayerServices(assemblies);
	}
}