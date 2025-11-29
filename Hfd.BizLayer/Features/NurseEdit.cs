using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Models;

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
        public string UserId { get; set; }

        public UserInfo? User { get; set; }

        /// <inheritdoc />
        public long? PlotId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage, IUserService service) : IRequestHandler<Request, INurseFull>
    {
        /// <inheritdoc />
        public async Task<INurseFull> Handle(Request request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                request.UserId = await service.Create(request.User, UserRole.Nurse) ?? throw new UserNotFoundException();
            }

            return await storage.GetService<INurseService>().Edit(request.Id, request);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.Id).GreaterThan(0).WithError(ErrorType.NotCorrect, HfdError.NurseId);
                this.RuleFor(x => x.UserId)
                    .NotEmpty()
                    .When(x => x.User == null || string.IsNullOrEmpty(x.User.PhoneNumber))
                    .WithError(ErrorType.NotCorrect, HfdError.NurseUserId);
            }
        }
    }
}
