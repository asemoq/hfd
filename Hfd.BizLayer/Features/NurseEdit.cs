using static Hfd.Common.Contract.Helpers.FieldFormatHelper;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Edit nurse.
/// </summary>
public static class NurseEdit
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : INurseInfo, IRequest<INurseFull>
    {
        /// <summary>
        ///     Идентификатор.
        /// </summary>
        public long Id { get; set; }

        /// <inheritdoc />
        public string FullName { get; set; }

        /// <inheritdoc />
        public string PhoneNumber { get; set; }

        /// <inheritdoc />
        public long? PlotNumber { get; set; }

        /// <inheritdoc />
        public long UserId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, INurseFull>
    {
        /// <inheritdoc />
        public async Task<INurseFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<INurseService>();
            return await service.Edit(request.Id, request);
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
