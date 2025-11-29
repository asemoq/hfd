namespace Hfd.BizLayer.Features;

/// <summary>
///     Get patient by user id.
/// </summary>
public static class PatientGetByUserId
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<IPatientFull>
    {
        public string UserId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IPatientFull>
    {
        /// <inheritdoc />
        public async Task<IPatientFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IPatientService>();
            return await service.GetByUserId(request.UserId);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.UserId).NotEmpty().WithError(ErrorType.NotCorrect, HfdError.PatientUserId);
            }
        }
    }
}
