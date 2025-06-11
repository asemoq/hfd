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
    public sealed class Handler : IRequestHandler<Request, bool>
    {
        public readonly IBizContext _context;

        public Handler(IBizContext context)
        {
            this._context = context;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = this._context.Services.GetRequiredService<IDbStorage>().GetService<ISurveyService>();
            return await service.NecessityByPatientId(request.PatientId);
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
