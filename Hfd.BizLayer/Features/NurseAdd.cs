namespace Hfd.BizLayer.Features;

/// <summary>
///     Add nurse.
/// </summary>
public static class NurseAdd
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : INurseInfo, IRequest<INurseFull>
    {
        /// <inheritdoc />
        public string UserId { get; set; }

        /// <inheritdoc />
        public long? PlotId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, INurseFull>
    {
        /// <inheritdoc />
        public async Task<INurseFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<INurseService>();
            return await service.Add(request);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.UserId).NotEmpty().WithMessage(nameof(HfdError.HFDNurseUserIdIsNotCorrect));
            }
        }
    }
}
