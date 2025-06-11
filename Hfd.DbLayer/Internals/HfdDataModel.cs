using Hfd.Common.Contract.Services;
using Hfd.Common.DbLayer.Internals;
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

    public class Factory(IUserAccessor userAccessor, ILogger<Factory> log)
        : DataModelFactory<HfdDataModel>(userAccessor)
    {
        /// <summary>
        ///     Журнал приложения.
        /// </summary>
        private readonly ILogger<Factory> log = log;

        public override HfdDataModel Create()
        {
            this.log.CreatingDataModel();

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

            return new HfdDataModel(options, null!);
        }
    }

    #endregion
}
