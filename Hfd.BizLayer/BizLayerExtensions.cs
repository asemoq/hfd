using System.Reflection;
using Hfd.BizLayer.Internals;
using Hfd.Common.BizLayer;
using Hfd.Common.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Hfd.BizLayer;

public static class BizLayerExtensions
{
    /// <summary>
    ///     Регистрация сервисов.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies">Сборки, в которых ищем сервисы.</param>
    /// <returns>Сервисы приложения.</returns>
    public static IServiceCollection AddHfdBizLayerServices(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly[] assemblies)
    {
        services.Configure<ProviderOptions>(configuration.GetSection("providerOptions"))
            .AddTransient<IProviderOptions>(p => p.GetRequiredService<IOptions<ProviderOptions>>().Value);
        return services.AddHfdCommonBizLayerServices(assemblies);
    }

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule,
        ErrorType type,
        HfdError error)
    {
        rule.WithMessage($"{type.ToString().ToSnakeCase()}.{error.ToString().ToSnakeCase()}");
        return rule;
    }
}
