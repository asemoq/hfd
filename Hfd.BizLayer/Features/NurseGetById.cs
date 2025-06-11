namespace Hfd.BizLayer.Features;

/// <summary>
///     Get nurse by id.
/// </summary>
public static class NurseGetById
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<NurseModel>
    {
        public long NurseId { get; set; }
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
            return this._context.GetMapper().Map<INurseFull, NurseModel>(await service.GetById(request.NurseId));
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.NurseId).GreaterThan(0).WithMessage(nameof(HfdError.HFDNurseIdIsNotCorrect));
            }
        }
    }
}
