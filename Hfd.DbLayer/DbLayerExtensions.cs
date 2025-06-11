global using System;
global using System.Reflection;
global using Microsoft.Extensions.Logging;
using Hfd.Common.DbLayer;
using Hfd.Common.DbLayer.Services;
using Hfd.DbLayer.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
        var factory = serviceProvider.GetService<IDataModelFactory<HfdDataModel>>();
        using var model = factory.Create();
        model.Database.Migrate();
        serviceProvider.GetService<ILogger<HfdDataModel>>()?.LogDebug("Disposed data model.");
    }
}
