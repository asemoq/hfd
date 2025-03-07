using System.Collections.Generic;
using Hfd.Base.Contract.Model;
using Hfd.Base.Contract.Services;
using Hfd.Contract.Model;
using ISkipTakeRq = Hfd.Base.Contract.Model.ISkipTakeRq;

namespace Hfd.Contract.Services;

/// <summary>
///     Сервис управления пациентами.
/// </summary>
public interface IPatientService : IDbService
{
	/// <summary>
	///     Сохранить пациента в БД.
	/// </summary>
	Task<IPatientFull> Add(IPatientInfo patientInfo);

	/// <summary>
	///     Изменить пациента в БД.
	/// </summary>
	Task<IPatientFull> Edit(long patientId, IPatientInfo patientInfo);

	/// <summary>
	///     Получить список пациентов из БД.
	/// </summary>
	Task<IPageList<IPatientFull>> GetAll(ISkipTakeRq request, long? nurseId);

	/// <summary>
	///     Получить список пациентов для опроса из БД.
	/// </summary>
	Task<List<IPatientFull>> GetForRequest();

	/// <summary>
	///     Получить пациента из БД по идентификатору.
	/// </summary>
	Task<IPatientFull> GetById(long patientId, bool includeDependencies = false);

	/// <summary>
	///     Получить пациента из БД по идентификатору пользователя.
	/// </summary>
	Task<IPatientFull> GetByUserId(long userId);

	/// <summary>
	///     Удалить пациента.
	/// </summary>
	Task<IPatientFull> Delete(long patientId);
}