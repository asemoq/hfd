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
    public sealed class Request : INurseInfo, IRequest<INurseFull>
    {
        /// <inheritdoc />
        public long? PlotNumber { get; set; }

        /// <inheritdoc />
        public long UserId { get; set; }

        /// <inheritdoc />
        public string PhoneNumber { get; set; }
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
                this.RuleFor(x => x.UserId).GreaterThan(0).WithMessage(nameof(HfdError.HFDNurseUserIdIsNotCorrect));
                this.RuleFor(x => x.PhoneNumber)
                    .Must(IsValidPhoneNumber)
                    .WithMessage(nameof(HfdError.HFDNursePhoneNumberIsNotValid));
            }
        }
    }
}
