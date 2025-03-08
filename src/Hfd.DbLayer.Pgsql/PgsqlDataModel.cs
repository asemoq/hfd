using Hfd.Base.Contract.Services;
using Hfd.Base.DbLayer.Internals;
using Hfd.DbLayer.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Hfd.DbLayer;

public class PgsqlDataModel : HfdDataModel
{
	public PgsqlDataModel(DbContextOptions<PgsqlDataModel> options, IUserAccessor userAccessor)
		: base(options, userAccessor)
	{
	}

	#region [ Factory ]

	public class Factory : DataModelFactory<HfdDataModel>
	{
		/// <summary>
		///     Журнал приложения.
		/// </summary>
		private readonly ILogger<Factory> _log;


		public Factory(IUserAccessor userAccessor, ILogger<Factory> log)
			: base(userAccessor)
		{
			_log = log;
		}

		public override HfdDataModel Create()
		{
			_log.LogDebug("Creating data model.");

			var options =
				new DbContextOptionsBuilder<PgsqlDataModel>()
					.UseNpgsql(ConnectionString, x => x.MigrationsHistoryTable("migration"))
					.Options;

			return new PgsqlDataModel(options, UserAccessor);
		}
	}

	#endregion

	#region [ DesignTimeFactory ]

	public class DesignTimeFactory : IDesignTimeDbContextFactory<PgsqlDataModel>
	{
		private const string designTimeConnectionString =
			"Host=localhost;Database=hfd;Username=postgres;Password=postgres";

		public PgsqlDataModel CreateDbContext(string[] args)
		{
			var options =
				new DbContextOptionsBuilder<PgsqlDataModel>()
					.UseNpgsql(designTimeConnectionString, x => x.MigrationsHistoryTable("migration"))
					.Options;

			return new PgsqlDataModel(options, null);
		}
	}

	#endregion
}