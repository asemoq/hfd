namespace Hfd.BizLayer.Features;

public static class SurveyNecessityByPatientId
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<bool>
    {
        /// <summary>
        ///     Id пациента.
        /// </summary>
        public long PatientId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, bool>
    {
        /// <inheritdoc />
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<ISurveyService>();
            return await service.NecessityByPatientId(request.PatientId);
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
