namespace Hfd.BizLayer.Features;

/// <summary>
///     Edit survey.
/// </summary>
public static class SurveyEdit
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : ISurveyInfo, IRequest<ISurveyFull>
    {
        /// <summary>
        ///     Идентификатор.
        /// </summary>
        public long Id { get; set; }

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
            return await service.Edit(request.Id, request);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.PatientId).GreaterThan(0).WithMessage(nameof(HfdError.HFDSurveyPatientIdIsNotCorrect));
            }
        }
    }
}
