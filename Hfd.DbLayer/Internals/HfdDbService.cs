namespace Hfd.DbLayer.Internals;

public class HfdDbService : DbService<HfdDbStorage, HfdDataModel>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="HfdDbService" /> class.
    /// </summary>
    /// <param name="storage">Storage.</param>
    /// <param name="log">Logger.</param>
    protected HfdDbService(HfdDbStorage storage, ILogger log)
        : base(storage, log)
    {
    }
}
