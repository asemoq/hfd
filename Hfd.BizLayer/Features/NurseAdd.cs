using Hfd.Common.Models;

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

        public UserInfo? User { get; set; }

        /// <inheritdoc />
        public long? PlotId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage, IUserService service, IBizContext context) : IRequestHandler<Request, INurseFull>
    {
        /// <inheritdoc />
        public async Task<INurseFull> Handle(Request request, CancellationToken cancellationToken)
        {
            if (request.User != null)
            {
                var user = await service.FindUser(request.User.PhoneNumber);
                if (user == null)
                {
                    request.User.Language = context.User.Language;
                    request.User.DefaultRole = UserRole.Nurse;
                    request.User.Roles = [UserRole.Nurse];
                    user = await service.CreateUser(request.User);
                }

                request.UserId = user.Id;
            }

            return await storage.GetService<INurseService>().Add(request);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.UserId).NotEmpty().When(x => x.User == null).WithMessage(nameof(HfdError.NurseUserIdIsNotCorrect));
            }
        }
    }
}
