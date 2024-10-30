global using System;
global using System.Reflection;
global using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Hfd.Base.DbLayer;
using Hfd.Base.DbLayer.Services;
using Hfd.DbLayer.Internals;
using LinqKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions;

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

	public static Expression<Func<T, bool>> AndExpression<T>(this Expression<Func<T, bool>> query,
		Expression<Func<T, bool>> exp)
	{
		return query != null ? query.And(exp) : exp;
	}
}