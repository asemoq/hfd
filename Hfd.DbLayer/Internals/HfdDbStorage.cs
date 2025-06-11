using Hfd.Common.DbLayer.Services;

namespace Hfd.DbLayer.Internals;

public class HfdDbStorage : DbStorage<HfdDataModel>
{
    public HfdDbStorage(IServiceProvider sp, ILogger<HfdDbStorage> log)
        : base(sp, log)
    {
        log.LogDebug("Application repository created");
    }

    /// <inheritdoc />
    protected override IDisposable BeginTransaction(HfdDataModel model)
    {
        return model.Database.BeginTransaction();
    }

    /// <inheritdoc />
    protected override void CommitTransaction(HfdDataModel model)
    {
        model.Database.CommitTransaction();
    }

    /// <inheritdoc />
    protected override void RollbackTransaction(HfdDataModel model)
    {
        model.Database.RollbackTransaction();
    }
}
