using Hfd.Common.Contract.Services;
using Hfd.Common.Models;
using Hfd.Contract.Model;

namespace Hfd.Contract.Services;

/// <summary>
///     Сервис управления медсестрами.
/// </summary>
public interface INurseService : IDbService
{
    /// <summary>
    ///     Сохранить медсестру в БД.
    /// </summary>
    Task<INurseFull> Add(INurseInfo nurseInfo);

    /// <summary>
    ///     Изменить медсестру в БД.
    /// </summary>
    Task<INurseFull> Edit(long nurseId, INurseInfo nurseInfo);

    /// <summary>
    ///     Получить список медсестер из БД.
    /// </summary>
    Task<IPageList<INurseFull>> GetAll(ISkipTakeRq request);

    /// <summary>
    ///     Получить медсестру из БД по идентификатору.
    /// </summary>
    Task<INurseFull> GetById(long nurseId);

    /// <summary>
    ///     Удалить медсестру.
    /// </summary>
    Task<INurseFull> Delete(long nurseId);
}
