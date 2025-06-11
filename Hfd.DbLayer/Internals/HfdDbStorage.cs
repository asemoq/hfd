namespace Hfd.DbLayer.Internals;

public class HfdDbStorage(IServiceProvider sp, ILogger<HfdDbStorage> log)
    : DbStorage<HfdDataModel>(sp, log)
{
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
