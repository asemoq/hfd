namespace Hfd.BizLayer.Features;

/// <summary>
///     Get patient by id.
/// </summary>
public static class PatientGetById
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<IPatientFull>
    {
        public long PatientId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IPatientFull>
    {
        /// <inheritdoc />
        public async Task<IPatientFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IPatientService>();
            return await service.GetById(request.PatientId);
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
