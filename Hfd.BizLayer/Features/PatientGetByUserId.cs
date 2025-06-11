namespace Hfd.BizLayer.Features;

/// <summary>
///     Get patient by user id.
/// </summary>
public static class PatientGetByUserId
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<PatientModel>
    {
        public long UserId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler : IRequestHandler<Request, PatientModel>
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
        public async Task<PatientModel> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = this._context.Services.GetRequiredService<IDbStorage>().GetService<IPatientService>();
            return this._context.GetMapper().Map<IPatientFull, PatientModel>(await service.GetByUserId(request.UserId));
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.UserId).GreaterThan(0).WithMessage(nameof(HfdError.HFDPatientUserIdIsNotCorrect));
            }
        }
    }
}
