using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Model;
using Hfd.Base.DbLayer;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Contract.Services;
using Hfd.DbLayer.Internals;
using Microsoft.EntityFrameworkCore;

namespace Hfd.DbLayer.Services;

public class SurveyDbService : HfdDbService, ISurveyService
{
	/// <summary>
	///     Constructor of Survey service class.
	/// </summary>
	/// <param name="storage"></param>
	/// <param name="log"></param>
	public SurveyDbService(HfdDbStorage storage, ILogger<SurveyDbService> log)
		: base(storage, log)
	{
	}

	public async Task<ISurveyModel> Add(ISurveyInfo surveyInfo)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var surveys = await model.Surveys
					.Where(x => x.PatientId == surveyInfo.PatientId && x.Date.Date == DateTime.UtcNow.Date)
					.ToListAsync();
				if (surveys.Count == 2)
					throw new HfdException(nameof(HfdError.HFD_Survey_Already_Exists));

				var survey = new SurveyEntity
				{
					Dyspnea = surveyInfo.Dyspnea,
					AdValue = surveyInfo.AdValue,
					WeightValue = surveyInfo.WeightValue,
					Edema = surveyInfo.Edema,
					HeartRateValue = surveyInfo.HeartRateValue,
					PositionInBed = surveyInfo.PositionInBed,
					PatientId = surveyInfo.PatientId,
					Date = DateTime.UtcNow,
					CreatedAt = DateTime.UtcNow
				};
				await model.Surveys.AddAsync(survey);
				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return survey;
			}
			catch
			{
				await trans.RollbackAsync();
				throw;
			}
		});
	}

	public async Task<ISurveyModel> Edit(long surveyId, ISurveyInfo surveyInfo)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var survey = await model.Surveys.SingleOrDefaultAsync(x => x.Id == surveyId);
				if (survey == null)
					throw new HfdException(nameof(HfdError.HFD_Survey_Not_Exists));

				survey.Dyspnea = surveyInfo.Dyspnea;
				survey.AdValue = surveyInfo.AdValue;
				survey.WeightValue = surveyInfo.WeightValue;
				survey.Edema = surveyInfo.Edema;
				survey.HeartRateValue = surveyInfo.HeartRateValue;
				survey.PositionInBed = surveyInfo.PositionInBed;
				survey.PatientId = surveyInfo.PatientId;
				survey.UpdatedAt = DateTime.UtcNow;
				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return survey;
			}
			catch
			{
				await trans.RollbackAsync();
				throw;
			}
		});
	}

	public async Task<IPageList<ISurveyModel>> GetAll(ISkipTakeRq request, long? patientId)
	{
		return await Execute(async model =>
		{
			Expression<Func<SurveyEntity, bool>> query = x => true;

			if (patientId != null)
				query = query.AndExpression(x => x.PatientId == patientId);

			var total = await model.Surveys.Where(query).CountAsync();
			var skip = Math.Min(total, request.Skip);
			var take = Math.Min(total - request.Skip, request.Take == 0 ? total : request.Take);
			return new PageList<ISurveyModel>
			{
				Total = total,
				First = skip,
				Items = take > 0
					? await model.Surveys
						.Include(x => x.PatientEntity)
						.ThenInclude(p => p.UserEntity)
						.AsNoTracking().Where(query)
						.Skip(skip)
						.Take(take)
						.ToListAsync<ISurveyModel>()
					: new List<ISurveyModel>()
			};
		});
	}

	public async Task<ISurveyModel> GetById(long surveyId)
	{
		return await Execute(async model =>
			await model.Surveys.FirstOrDefaultAsync(x => x.Id == surveyId) ??
			throw new HfdException(nameof(HfdError.HFD_Survey_Not_Found)));
	}

	public async Task<ISurveyModel> Delete(long surveyId)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var survey = await model.Surveys.SingleOrDefaultAsync(x => x.Id == surveyId);
				if (survey == null)
					throw new HfdException(nameof(HfdError.HFD_Survey_Not_Exists));
				model.Remove(survey);
				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return survey;
			}
			catch
			{
				await trans.RollbackAsync();
				throw;
			}
		});
	}
}