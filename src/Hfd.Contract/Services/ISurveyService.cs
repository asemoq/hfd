using System.Collections.Generic;
using Hfd.Common.ApiClient.Model;
using Hfd.Common.Contract.Services;
using Hfd.Contract.Model;

namespace Hfd.Contract.Services;

/// <summary>
///     Сервис управления опросами.
/// </summary>
public interface ISurveyService : IDbService
{
	/// <summary>
	///     Сохранить опрос в БД.
	/// </summary>
	Task<ISurveyFull> Add(ISurveyInfo surveyInfo);

	/// <summary>
	///     Изменить опрос в БД.
	/// </summary>
	Task<ISurveyFull> Edit(long surveyId, ISurveyInfo surveyInfo);

	/// <summary>
	///     Получить список опросов из БД.
	/// </summary>
	Task<IPageList<ISurveyFull>> GetAll(ISkipTakeRq request, long? patientId);

	/// <summary>
	///     Получить список опросов из БД.
	/// </summary>
	Task<IEnumerable<ISurveyFull>> GetForPeriod(long patientId, int month, int year);

	/// <summary>
	///     Получить последний пройденный опрос по идентификатору пациента.
	/// </summary>
	Task<ISurveyFull> GetLastByPatientId(long patientId);

	/// <summary>
	///     Получить признак необходимости пройти опрос по идентификатору пациента.
	/// </summary>
	Task<bool> NecessityByPatientId(long patientId);

	/// <summary>
	///     Получить опрос из БД по идентификатору.
	/// </summary>
	Task<ISurveyFull> GetById(long surveyId);

	/// <summary>
	///     Удалить опрос.
	/// </summary>
	Task<ISurveyFull> Delete(long surveyId);
}