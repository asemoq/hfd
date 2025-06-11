using Hfd.Common.ApiClient.Model;
using Hfd.Common.Contract.Enums;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Contract.Internals;
using Hfd.Common.Contract.Model;

namespace Hfd.BizLayer.Features;

public static class UserAdd
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : UserInfo, IRequest<IUserFull>
    {
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IUserFull>
    {
        /// <inheritdoc />
        public async Task<IUserFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IUserLoginDbService>();
            return await service.Create(Enum.Parse<LoginRole>(request.Usertype), request, request.Password!.Hash());
        }
    }

    /// <inheritdoc />
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            this.RuleFor(x => x.Password).NotEmpty().WithMessage(nameof(HfdCoreError.UserPasswordCannotBeEmpty));
            this.RuleFor(x => x.Username).NotEmpty().WithMessage(nameof(HfdCoreError.UserUsernameCannotBeEmpty));
            this.RuleFor(x => x.UniqueId).NotEmpty().WithMessage(nameof(HfdCoreError.UserUniqueIdCannotBeEmpty));
            this.RuleFor(x => x.FullName).NotEmpty().WithMessage(nameof(HfdCoreError.UserFullNameCannotBeEmpty));
        }
    }
}
