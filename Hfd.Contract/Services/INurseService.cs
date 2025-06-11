using Hfd.Common.ApiClient.Model;
using Hfd.Common.Contract.Services;
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
    ///     Получить медсестру из БД по идентификатору пользователя.
    /// </summary>
    Task<INurseFull> GetByUserId(long userId);

    /// <summary>
    ///     Удалить медсестру.
    /// </summary>
    Task<INurseFull> Delete(long nurseId);
}
