using System.Reflection;
using Hfd.Common.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Contract;

public static class ContractExtensions
{
    /// <summary>
    ///     Регистрация сервисов репозитория.
    /// </summary>
    /// <param name="assemblies">Сборки, в которых ищем поставщиков данных.</param>
    /// <returns>Измененные сервисы приложения.</returns>
    public static IServiceCollection AddHfdContractServices(this IServiceCollection services, Assembly[] assemblies)
    {
        return services.AddHfdCommonContractServices(assemblies);
    }
}
