using static Hfd.Common.Contract.Helpers.FieldFormatHelper;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Add nurse.
/// </summary>
public static class NurseAdd
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : INurseInfo, IRequest<NurseModel>
    {
        /// <inheritdoc />
        public long? PlotNumber { get; set; }

        /// <inheritdoc />
        public long UserId { get; set; }

        /// <inheritdoc />
        public string PhoneNumber { get; set; }
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
            return this._context.GetMapper().Map<INurseFull, NurseModel>(await service.Add(request));
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.UserId).GreaterThan(0).WithMessage(nameof(HfdError.HFDNurseUserIdIsNotCorrect));
                this.RuleFor(x => x.PhoneNumber)
                    .Must(IsValidPhoneNumber)
                    .WithMessage(nameof(HfdError.HFDNursePhoneNumberIsNotValid));
            }
        }
    }
}
