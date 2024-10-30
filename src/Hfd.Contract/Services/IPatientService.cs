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
	Task<IPatientModel> Add(IPatientInfo patientInfo, IPatientCardInfo patientCardInfo);

	/// <summary>
	///     Изменить пациента в БД.
	/// </summary>
	Task<IPatientModel> Edit(long patientId, IPatientInfo patientInfo, IPatientCardInfo patientCardInfo);

	/// <summary>
	///     Получить список пациентов из БД.
	/// </summary>
	Task<IPageList<IPatientModel>> GetAll(ISkipTakeRq request);

	/// <summary>
	///     Получить список пациентов для опроса из БД.
	/// </summary>
	Task<List<IPatientFull>> GetForRequest();

	/// <summary>
	///     Получить пациента из БД по идентификатору.
	/// </summary>
	Task<IPatientModel> GetById(long patientId);

	/// <summary>
	///     Удалить пациента.
	/// </summary>
	Task<IPatientModel> Delete(long patientId);
}