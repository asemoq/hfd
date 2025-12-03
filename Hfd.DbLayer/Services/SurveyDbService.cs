using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Models;
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
                    .Where(x => x.PatientId == surveyInfo.PatientId && x.DateTime.ToLocalTime().Date == DateTime.Now.Date)
                    .ToListAsync();
                if (surveys.Count == 2) throw new AlreadyExistsException(HfdError.Survey);

                var weight = WeightType.Normal;
                var minWeekWeight = await model.Surveys
                    .Where(x => x.PatientId == surveyInfo.PatientId
                                && x.DateTime.ToLocalTime().Date >= DateTime.Now.Date.AddDays(-7)
                                && x.DateTime.ToLocalTime().Date <= DateTime.Now.Date)
                    .Select(x => x.WeightValue)
                    .MinAsync();
                if (surveyInfo.WeightValue > minWeekWeight * SurveyConstants.WeightGainCoefficient) weight = WeightType.WeightGain;

                var survey = new SurveyEntity
                {
                    Dyspnea = surveyInfo.Dyspnea,
                    AdValue = surveyInfo.AdValue,
                    WeightValue = surveyInfo.WeightValue,
                    Weight = weight,
                    Edema = surveyInfo.Edema,
                    HeartRateValue = surveyInfo.HeartRateValue,
                    PositionInBed = surveyInfo.PositionInBed,
                    PatientId = surveyInfo.PatientId,
                    DateTime = DateTime.UtcNow,
                    DayTimeValue = DateTime.Now.ToString("tt", CultureInfo.InvariantCulture),
                    CreatedAt = DateTime.UtcNow,
                };

                if (surveys.Count == 1 && surveys.Single().DateType == survey.DateType) throw new AlreadyExistsException(HfdError.Survey);
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
                if (survey == null) throw new NotFoundException(HfdError.Survey);

                survey.Dyspnea = surveyInfo.Dyspnea;
                survey.AdValue = surveyInfo.AdValue;
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
                        .AsNoTracking()
                        .Where(query)
                        .Skip(skip)
                        .Take(take)
                        .OrderBy(x => x.Id)
                        .ToListAsync<ISurveyFull>()
                    : new List<ISurveyFull>(),
            };
        });
    }

    public async Task<List<ISurveyFull>> GetForPeriod(long patientId, int month, int year)
    {
        return await this.Execute(async model
            => await model.Surveys.Where(x => x.PatientId == patientId && x.DateTime.Month == month && x.DateTime.Year == year)
                .ToListAsync<ISurveyFull>());
    }

    public async Task<ISurveyFull> GetLastByPatientId(long patientId)
    {
        return await this.Execute(async model
            => await model.Surveys.OrderByDescending(x => x.DateTime).FirstOrDefaultAsync(x => x.PatientId == patientId)
               ?? throw new NotFoundException(HfdError.Survey));
    }

    public async Task<bool> NecessityByPatientId(long patientId)
    {
        return await this.Execute(async model =>
        {
            return !await model.Surveys.AnyAsync(s
                => s.DateTime.ToLocalTime().Date == DateTime.Now.Date
                   && s.DayTimeValue == DateTime.Now.DateType()
                   && s.PatientId == patientId);
        });
    }

    public async Task<ISurveyFull> GetById(long surveyId)
    {
        return await this.Execute(async model => await model.Surveys.SingleOrDefaultAsync(x => x.Id == surveyId)
                                                 ?? throw new NotFoundException(HfdError.Survey));
    }

    public async Task<ISurveyFull> Delete(long surveyId)
    {
        return await this.Execute(async model =>
        {
            await using var trans = await model.Database.BeginTransactionAsync();
            try
            {
                var survey = await model.Surveys.SingleOrDefaultAsync(x => x.Id == surveyId);
                if (survey == null) throw new NotFoundException(HfdError.Survey);

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
