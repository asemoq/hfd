using System.Linq.Expressions;
using System.Reflection;
using Hfd.Base.Contract.Services;
using Hfd.Base.DbLayer.Internals;
using Hfd.Base.DbLayer.Services;
using LinqKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.Base.DbLayer;

public static class DbLayerExtensions
{
	public static Expression<Func<T, bool>> AndExpression<T>(this Expression<Func<T, bool>>? query,
		Expression<Func<T, bool>> exp)
	{
		return query != null ? query.And(exp) : exp;
	}

	#region Регистрация сервисов репозитория

	/// <summary>
	///     Регистрация сервисов репозитория.
	/// </summary>
	/// <typeparam name="TDbStorage">Тип репозитория данных.</typeparam>
	/// <typeparam name="TDataModel">Тип модели данных.</typeparam>
	/// <param name="services">Сервисы приложения.</param>
	/// <param name="configuration">Конфигурация приложения.</param>
	/// <param name="assemblies">Сборки, в которых ищем поставщиков данных.</param>
	/// <returns>Измененные сервисы приложения.</returns>
	public static IServiceCollection AddHfdBaseDbLayerServices<TDbStorage, TDataModel>(this IServiceCollection services,
		IConfiguration configuration, Assembly[] assemblies)
		where TDbStorage : DbStorage<TDataModel>
		where TDataModel : class, IDataModel
	{
		var assemblyName = configuration.GetValue<string>("dataModel:loadFromAssembly");
		var assembly = Assembly.Load(assemblyName);

		var addDataModelFactory = assembly
			.GetType($"{assemblyName}Extensions")
			.GetMethod("AddDataModelFactory", BindingFlags.Public | BindingFlags.Static);

		addDataModelFactory.Invoke(null, new[] { services });
		assemblies = assemblies.Append(assembly).ToArray();

		return services
			.AddSingleton<IDbServiceTypeMap>(_ => new DbServiceTypeMap<TDbStorage, TDataModel>(assemblies))
			.AddScoped<TDbStorage>()
			.AddTransient<IDbStorage>(p => p.GetRequiredService<TDbStorage>())
			.AddTransient<IDbServiceFactory, DbServiceFactory>();
	}

	/// <inheritdoc cref="IDbStorage.BeginTransaction" />
	public static void BeginTransaction(this IDbService service)
	{
		service.Storage.BeginTransaction();
	}

	/// <inheritdoc cref="IDbStorage.CommitTransaction" />
	public static void CommitTransaction(this IDbService service)
	{
		service.Storage.CommitTransaction();
	}

	/// <inheritdoc cref="IDbStorage.RollbackTransaction" />
	public static void RollbackTransaction(this IDbService service)
	{
		service.Storage.RollbackTransaction();
	}

	/// <inheritdoc cref="IDbStorage.Get{TDbService}" />
	public static TDbService Get<TDbService>(this IDbService service)
		where TDbService : IDbService
	{
		return service.Storage.Get<TDbService>();
	}

	#endregion
}