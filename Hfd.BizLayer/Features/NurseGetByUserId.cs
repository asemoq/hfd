namespace Hfd.BizLayer.Features;

/// <summary>
///     Get nurse by user id.
/// </summary>
public static class NurseGetByUserId
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<INurseFull>
    {
        public string UserId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, INurseFull>
    {
        /// <inheritdoc />
        public async Task<INurseFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<INurseService>();
            return await service.GetByUserId(request.UserId);
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
