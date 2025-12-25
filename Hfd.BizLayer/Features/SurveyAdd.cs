namespace Hfd.BizLayer.Features;

/// <summary>
///     Add survey.
/// </summary>
public static class SurveyAdd
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : ISurveyInfo, IRequest<ISurveyFull>
    {
        /// <inheritdoc />
        public DyspneaType Dyspnea { get; set; }

        /// <inheritdoc />
        public double AdValue { get; set; }

        /// <inheritdoc />
        public double WeightValue { get; set; }

        /// <inheritdoc />
        public EdemaType Edema { get; set; }

        /// <inheritdoc />
        public double HeartRateValue { get; set; }

        /// <inheritdoc />
        public PositionInBedType PositionInBed { get; set; }

        /// <inheritdoc />
        public long PatientId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, ISurveyFull>
    {
        /// <inheritdoc />
        public async Task<ISurveyFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<ISurveyService>();
            return await service.Add(request);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.PatientId).GreaterThan(0).WithError(ErrorType.NotCorrect, HfdError.SurveyPatientId);
                this.RuleFor(x => x.AdValue).LessThanOrEqualTo(180).WithError(ErrorType.NotCorrect, HfdError.SurveyAd);
                this.RuleFor(x => x.WeightValue).LessThanOrEqualTo(700).WithError(ErrorType.NotCorrect, HfdError.SurveyWeight);
                this.RuleFor(x => x.HeartRateValue).LessThanOrEqualTo(220).WithError(ErrorType.NotCorrect, HfdError.SurveyHeartRate);
            }
        }
    }
}
