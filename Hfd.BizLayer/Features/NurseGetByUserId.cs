namespace Hfd.BizLayer.Features;

/// <summary>
///     Get nurse by user id.
/// </summary>
public static class NurseGetByUserId
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<NurseModel>
    {
        public long UserId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler : IRequestHandler<Request, NurseModel>
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
        public async Task<NurseModel> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = this._context.Services.GetRequiredService<IDbStorage>().GetService<INurseService>();
            return this._context.GetMapper().Map<INurseFull, NurseModel>(await service.GetByUserId(request.UserId));
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.UserId).GreaterThan(0).WithMessage(nameof(HfdError.HFDNurseUserIdIsNotCorrect));
            }
        }
    }
}
