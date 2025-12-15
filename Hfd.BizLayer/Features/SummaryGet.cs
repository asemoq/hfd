using System.Linq;
using AutoMapper;
using Hfd.Models;

namespace Hfd.BizLayer.Features;

public static class SummaryGet
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<List<SurveyModel>>
    {
        /// <summary>
        ///     Id пациента.
        /// </summary>
        public long PatientId { get; set; }

        /// <summary>
        ///     Месяц и год.
        /// </summary>
        public DateTime Date { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IMapper mapper, IDbStorage storage) : IRequestHandler<Request, List<SurveyModel>>
    {
        /// <inheritdoc />
        public async Task<List<SurveyModel>> Handle(Request request, CancellationToken cancellationToken)
        {
            var list = new List<SurveyModel>();
            var startDate = new DateTime(request.Date.Year, request.Date.Month, 1);
            if (startDate > DateTime.Today) return list;
            var surveys = await storage.GetService<ISurveyService>().GetForPeriod(request.PatientId, request.Date.Month, request.Date.Year);
            var currentDate = startDate;
            while (currentDate.Month == startDate.Month && currentDate.Year == startDate.Year)
            {
                if (currentDate > DateTime.Today) break;
                foreach (DateType dateType in Enum.GetValues(typeof(DateType)))
                {
                    var hours = dateType == DateType.AM ? 4 : 18;
                    var model = new SurveyModel() { DateTime = currentDate.AddHours(hours), DateType = dateType.ToString(), };
                    var survey = surveys.SingleOrDefault(x => x.DateType == dateType && x.DateTime.Date == model.DateTime.Date);
                    if (survey is null)
                    {
                        list.Add(model);
                        continue;
                    }

                    list.Add(mapper.Map<SurveyModel>(survey));
                }

                currentDate = currentDate.AddDays(1);
            }

            return list;
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.PatientId).GreaterThan(0).WithError(ErrorType.NotCorrect, HfdError.SurveyPatientId);
            }
        }
    }
}
