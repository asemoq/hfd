namespace Hfd.BizLayer.Features;

/// <summary>
///     Get patient by user id.
/// </summary>
public static class PatientGetForReminder
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<IEnumerable<IPatientFull>>
    {
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IEnumerable<IPatientFull>>
    {
        /// <inheritdoc />
        public async Task<IEnumerable<IPatientFull>> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IPatientService>();
            return await service.GetForReminder();
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
            }
        }
    }
}
