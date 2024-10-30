using System.Reflection;
using Hfd.Base.Contract.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Base.Contract;

public static class ContractExtensions
{
	/// <summary>
	///     Регистрация сервисов репозитория.
	/// </summary>
	/// <param name="assemblies">Сборки, в которых ищем поставщиков данных.</param>
	/// <returns>Измененные сервисы приложения.</returns>
	public static IServiceCollection AddHfdBaseContractServices(this IServiceCollection services,
		Assembly[] assemblies)
	{
		return services
			.AddNamedServices(assemblies)
			.AddAutoMapper(assemblies);
	}
}