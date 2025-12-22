using System.Linq;
using AutoMapper;
using Hfd.Models;

namespace Hfd.BizLayer.Features;

public static class SummaryGetByIndicator
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<List<SurveyChartModel>?>
    {
        /// <summary>
        ///     Id пациента.
        /// </summary>
        public long PatientId { get; set; }

        /// <summary>
        ///     Индикатор.
        /// </summary>
        public Indicator Indicator { get; set; }

        /// <summary>
        ///     Начало периода.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     Конец периода.
        /// </summary>
        public DateTime FinishDate { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IMapper mapper, IDbStorage storage) : IRequestHandler<Request, List<SurveyChartModel>?>
    {
        /// <inheritdoc />
        public async Task<List<SurveyChartModel>?> Handle(Request request, CancellationToken cancellationToken)
        {
            if (request.StartDate > request.FinishDate) return null;
            var period = request.FinishDate - request.StartDate;
            if (period.TotalDays <= 0) return null;
            var surveys = await storage.GetService<ISurveyService>().GetForPeriod(request.PatientId, request.StartDate, request.FinishDate);
            return surveys.OrderBy(x => x.DateTime)
                .Select(s => new SurveyChartModel()
                {
                    Date = s.DateTime.ToLocalTime(),
                    Value = request.Indicator switch
                    {
                        Indicator.Weight => s.WeightValue,
                        Indicator.Ad => s.AdValue,
                        Indicator.HeartRate => s.HeartRateValue,
                        _ => 0,
                    },
                })
                .ToList();
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
