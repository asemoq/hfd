namespace Hfd.BizLayer.Features;

/// <summary>
///     Edit survey.
/// </summary>
public static class SurveyEdit
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : ISurveyInfo, IRequest<SurveyModel>
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
    public sealed class Handler : IRequestHandler<Request, SurveyModel>
    {
        private readonly IBizContext _context;

        /// <summary>
        ///     Creates an instance of <see cref="Handler" />.
        /// </summary>
        public Handler(IBizContext context)
        {
            this._context = context;
        }

        /// <inheritdoc />
        public async Task<SurveyModel> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = this._context.Services.GetRequiredService<IDbStorage>().GetService<ISurveyService>();
            return this._context.GetMapper().Map<ISurveyFull, SurveyModel>(await service.Edit(request.Id, request));
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
