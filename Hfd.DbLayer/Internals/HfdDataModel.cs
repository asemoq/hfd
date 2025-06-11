using Hfd.Common.Contract.Services;
using Hfd.Common.DbLayer.Internals;
using Hfd.Common.DbLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Hfd.DbLayer.Internals;

public partial class HfdDataModel : DataModel
{
    protected HfdDataModel(DbContextOptions options, IUserAccessor userAccessor)
        : base(options, userAccessor)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HfdDataModel).Assembly);
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
            this._log = log;
        }

        public override HfdDataModel Create()
        {
            this._log.LogDebug("Creating data model.");

            var options = new DbContextOptionsBuilder<HfdDataModel>().UseNpgsql(
                    this.ConnectionString,
                    x => x.MigrationsHistoryTable("migration"))
                .Options;

            return new HfdDataModel(options, this.UserAccessor);
        }
    }

    #endregion

    #region [ DesignTimeFactory ]

    public class DesignTimeFactory : IDesignTimeDbContextFactory<HfdDataModel>
    {
        private const string DesignTimeConnectionString = "Host=localhost;Database=hfd;Username=postgres;Password=postgres";

        public HfdDataModel CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<HfdDataModel>().UseNpgsql(
                    DesignTimeConnectionString,
                    x => x.MigrationsHistoryTable("migration"))
                .Options;

            return new HfdDataModel(options, null);
        }
    }

    #endregion
}
