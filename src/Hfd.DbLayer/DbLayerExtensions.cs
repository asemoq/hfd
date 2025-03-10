global using System;
global using System.Reflection;
global using Microsoft.Extensions.Logging;
using Hfd.Common.DbLayer;
using Hfd.Common.DbLayer.Services;
using Hfd.DbLayer.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.DbLayer;

public static class DbLayerExtensions
{
	public static IServiceCollection AddHfdDbLayerServices(this IServiceCollection services,
		IConfiguration configuration, params Assembly[] assemblies)
	{
		return services.AddHfdBaseDbLayerServices<HfdDbStorage, HfdDataModel>(configuration, assemblies);
	}

	public static void MigrateHfdDbLayer(this IServiceProvider serviceProvider)
	{
		var factory = serviceProvider.GetService<IDataModelFactory<HfdDataModel>>();
		using var model = factory.Create();
		model.Database.Migrate();
		serviceProvider.GetService<ILogger<HfdDataModel>>()?
			.LogDebug("Disposed data model.");
	}
}