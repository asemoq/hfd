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
        public long? PlotNumber { get; set; }

        public string UserId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, INurseFull>
    {
        /// <inheritdoc />
        public async Task<INurseFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<INurseService>();
            return await service.Edit(request.Id, request, request.UserId);
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
