namespace Hfd.BizLayer.Features;

public static class SurveyGetLastByPatientId
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<ISurveyFull>
    {
        /// <summary>
        ///     Id пациента.
        /// </summary>
        public long PatientId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, ISurveyFull>
    {
        /// <inheritdoc />
        public async Task<ISurveyFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<ISurveyService>();
            return await service.GetLastByPatientId(request.PatientId);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.PatientId).GreaterThan(0).WithMessage(nameof(HfdError.PatientIdIsNotCorrect));
            }
        }
    }
}
