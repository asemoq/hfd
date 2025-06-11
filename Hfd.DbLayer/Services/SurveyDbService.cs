using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hfd.Common.ApiClient.Model;
using Hfd.Common.Contract.Exceptions;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Contract.Services;

namespace Hfd.DbLayer.Services;

public class SurveyDbService(HfdDbStorage storage, ILogger<SurveyDbService> log)
    : HfdDbService(storage, log), ISurveyService
{
    public async Task<ISurveyFull> Add(ISurveyInfo surveyInfo)
    {
        return await this.Execute(async model =>
        {
            await using var trans = await model.Database.BeginTransactionAsync();
            try
            {
                var surveys = await model.Surveys
                    .Where(x => x.PatientId == surveyInfo.PatientId && x.DateTime.Date == DateTime.UtcNow.Date)
                    .ToListAsync();
                if (surveys.Count == 2)
                {
                    throw new HfdException(nameof(HfdError.HFDSurveyAlreadyExists));
                }

                var survey = new SurveyEntity
                {
                    Dyspnea = surveyInfo.Dyspnea,
                    AdValue = surveyInfo.AdValue,
                    WeightValue = surveyInfo.WeightValue,
                    Edema = surveyInfo.Edema,
                    HeartRateValue = surveyInfo.HeartRateValue,
                    PositionInBed = surveyInfo.PositionInBed,
                    PatientId = surveyInfo.PatientId,
                    DateTime = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                };

                if (surveys.Count == 1)
                {
                    if (surveys.Single().DayTime == survey.DayTime)
                    {
                        throw new HfdException(nameof(HfdError.HFDSurveyAlreadyExists));
                    }
                }

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

    public async Task<ISurveyFull> Edit(long surveyId, ISurveyInfo surveyInfo)
    {
        return await this.Execute(async model =>
        {
            await using var trans = await model.Database.BeginTransactionAsync();
            try
            {
                var survey = await model.Surveys.SingleOrDefaultAsync(x => x.Id == surveyId);
                if (survey == null)
                {
                    throw new HfdException(nameof(HfdError.HFDSurveyNotExists));
                }

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

    public async Task<IPageList<ISurveyFull>> GetAll(ISkipTakeRq request, long? patientId)
    {
        return await this.Execute(async model =>
        {
            Expression<Func<SurveyEntity, bool>> query = x => true;
            query = query.AndIf(patientId != null, x => x.PatientId == patientId);
            var total = await model.Surveys.Where(query).CountAsync();
            var skip = Math.Min(total, request.Skip);
            var take = Math.Min(total - request.Skip, request.Take == 0 ? total : request.Take);
            return new PageList<ISurveyFull>
            {
                Total = total,
                First = skip,
                Items = take > 0
                    ? await model.Surveys.Include(x => x.PatientEntity)
                        .ThenInclude(p => p!.UserEntity)
                        .AsNoTracking()
                        .Where(query)
                        .Skip(skip)
                        .Take(take)
                        .ToListAsync<ISurveyFull>()
                    : new List<ISurveyFull>(),
            };
        });
    }

    public async Task<IEnumerable<ISurveyFull>> GetForPeriod(long patientId, int month, int year)
    {
        return await this.Execute(async model
            => await model.Surveys.Where(x => x.PatientId == patientId && x.DateTime.Month == month && x.DateTime.Year == year)
                .ToListAsync());
    }

    public async Task<ISurveyFull> GetLastByPatientId(long patientId)
    {
        return await this.Execute(async model
            => await model.Surveys.OrderByDescending(x => x.DateTime).FirstOrDefaultAsync(x => x.PatientId == patientId)
               ?? throw new HfdException(nameof(HfdError.HFDSurveyNotFound)));
    }

    public async Task<bool> NecessityByPatientId(long patientId)
    {
        return await this.Execute(async model =>
        {
            var surveys = await model.Surveys.Where(x => x.PatientId == patientId && x.DateTime.Date == DateTime.UtcNow.Date)
                .ToListAsync();
            var dayTime = DateTime.UtcNow.Hour < 12 ? DayTime.Morning : DayTime.Afternoon;
            switch (surveys.Count)
            {
                case > 2:
                case 2:
                case 1 when surveys.Single().DayTime == dayTime:
                    return false;
            }

            return true;
        });
    }

    public async Task<ISurveyFull> GetById(long surveyId)
    {
        return await this.Execute(async model => await model.Surveys.SingleOrDefaultAsync(x => x.Id == surveyId)
                                                 ?? throw new HfdException(nameof(HfdError.HFDSurveyNotFound)));
    }

    public async Task<ISurveyFull> Delete(long surveyId)
    {
        return await this.Execute(async model =>
        {
            await using var trans = await model.Database.BeginTransactionAsync();
            try
            {
                var survey = await model.Surveys.SingleOrDefaultAsync(x => x.Id == surveyId);
                if (survey == null)
                {
                    throw new HfdException(nameof(HfdError.HFDSurveyNotExists));
                }

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
