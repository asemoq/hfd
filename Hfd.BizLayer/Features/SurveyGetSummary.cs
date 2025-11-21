namespace Hfd.BizLayer.Features;

public static class SurveyGetSummary
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<IEnumerable<ISurveyFull>>
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
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IEnumerable<ISurveyFull>>
    {
        /// <inheritdoc />
        public async Task<IEnumerable<ISurveyFull>> Handle(Request request, CancellationToken cancellationToken)
            => await storage.GetService<ISurveyService>().GetForPeriod(request.PatientId, request.Date.Month, request.Date.Year);

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
