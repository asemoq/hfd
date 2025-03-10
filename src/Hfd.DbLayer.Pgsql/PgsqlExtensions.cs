using Hfd.Common.DbLayer.Services;
using Hfd.DbLayer.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.DbLayer;

public static class PgsqlExtensions
{
	public static IServiceCollection AddDataModelFactory(this IServiceCollection services)
	{
		return services.AddTransient<IDataModelFactory<HfdDataModel>, PgsqlDataModel.Factory>();
	}
}