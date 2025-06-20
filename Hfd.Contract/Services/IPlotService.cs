using System.Threading;
using Hfd.Common.Contract.Services;
using Hfd.Common.Models;
using Hfd.Contract.Model;

namespace Hfd.Contract.Services;

/// <summary>
///     Сервис управления участками.
/// </summary>
public interface IPlotService : IDbService
{
    /// <summary>
    ///     Сохранить участок в БД.
    /// </summary>
    Task<IPlotFull> Add(IPlotInfo plotInfo);

    /// <summary>
    ///     Изменить участок в БД.
    /// </summary>
    Task<IPlotFull> Edit(long plotId, IPlotInfo plotInfo);

    /// <summary>
    ///     Получить список участков из БД.
    /// </summary>
    Task<IPageList<IPlotFull>> GetAll(ISkipTakeRq request);

    /// <summary>
    ///     Получить участок из БД по идентификатору.
    /// </summary>
    Task<IPlotFull> GetById(long plotId, bool includeDeps = false);

    /// <summary>
    ///     Удалить участок.
    /// </summary>
    Task<IPlotFull> Delete(long plotId);
}
