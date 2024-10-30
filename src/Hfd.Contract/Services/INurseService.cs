using Hfd.Base.Contract.Model;
using Hfd.Base.Contract.Services;
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
	Task<INurseModel> Add(INurseInfo nurseInfo);

	/// <summary>
	///     Изменить медсестру в БД.
	/// </summary>
	Task<INurseModel> Edit(long nurseId, INurseInfo nurseInfo);

	/// <summary>
	///     Получить список медсестер из БД.
	/// </summary>
	Task<IPageList<INurseModel>> GetAll(ISkipTakeRq request);

	/// <summary>
	///     Получить медсестру из БД по идентификатору.
	/// </summary>
	Task<INurseModel> GetById(long nurseId);

	/// <summary>
	///     Удалить медсестру.
	/// </summary>
	Task<INurseModel> Delete(long nurseId);
}