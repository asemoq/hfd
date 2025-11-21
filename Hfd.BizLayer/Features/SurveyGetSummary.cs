using System.Linq;
using AutoMapper;
using Hfd.Models;

namespace Hfd.BizLayer.Features;

public static class SurveyGetSummary
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
        [Obsolete("Obsolete")]
        public async Task<List<SurveyModel>> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<ISurveyService>();
            var surveys = await service.GetForPeriod(request.PatientId, request.Date.Month, request.Date.Year);
            var list = new List<SurveyModel>();
            var startDate = new DateOnly(request.Date.Year, request.Date.Month, 1);
            var currentDate = startDate;
            while (currentDate.Month == startDate.Month && currentDate.Year == startDate.Year)
            {
                foreach (DayTime dayTime in Enum.GetValues(typeof(DayTime)))
                {
                    var model = new SurveyModel()
                    {
                        DateTime = currentDate.ToDateTime(new TimeOnly((int)dayTime + 2, 0)), DayTime = dayTime.ToString(),
                    };

                    var survey = surveys.SingleOrDefault(x => x.DayTime == dayTime && x.DateTime.Date == model.DateTime.Date);
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
                this.RuleFor(x => x.PatientId).GreaterThan(0).WithMessage(nameof(HfdError.HFDPatientIdIsNotCorrect));
            }
        }
    }
}
