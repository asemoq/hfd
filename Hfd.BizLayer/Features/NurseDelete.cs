namespace Hfd.BizLayer.Features;

/// <summary>
///     Delete nurse.
/// </summary>
public static class NurseDelete
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<INurseFull>
    {
        public long NurseId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, INurseFull>
    {
        /// <inheritdoc />
        public async Task<INurseFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<INurseService>();
            return await service.Delete(request.NurseId);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.NurseId).GreaterThan(0).WithMessage(nameof(HfdError.NurseIdIsNotCorrect));
            }
        }
    }
}
