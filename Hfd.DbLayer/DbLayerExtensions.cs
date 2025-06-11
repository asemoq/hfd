global using System;
global using System.Reflection;
global using Hfd.Common.Contract;
global using Hfd.Common.DbLayer;
global using Hfd.Common.DbLayer.Services;
global using Hfd.DbLayer.Internals;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

namespace Hfd.DbLayer;

public static class DbLayerExtensions
{
    public static IServiceCollection AddHfdDbLayerServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddTransient<IDataModelFactory<HfdDataModel>, HfdDataModel.Factory>();
        return services.AddHfdCommonDbLayerServices<HfdDbStorage, HfdDataModel>(assemblies);
    }

    public static void MigrateHfdDbLayer(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var factory = scope.ServiceProvider.GetService<IDataModelFactory<HfdDataModel>>();
        using var model = factory?.Create();
        model?.Database.Migrate();
        serviceProvider.GetService<ILogger<HfdDataModel>>()?.DisposedDataModel();
    }
}
