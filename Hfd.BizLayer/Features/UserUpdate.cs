using Hfd.Common.ApiClient.Model;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Contract.Model;

namespace Hfd.BizLayer.Features;

public static class UserUpdate
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : UserInfo, IRequest<IUserFull>
    {
        /// <summary>
        ///     Идентификатор пользователя.
        /// </summary>
        public long Id { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IUserFull>
    {
        /// <inheritdoc />
        public async Task<IUserFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IUserLoginDbService>();
            return await service.Update(request.Id, request);
        }
    }

    /// <inheritdoc />
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            this.RuleFor(x => x.Id).GreaterThan(0).WithMessage(nameof(HfdCoreError.UserIdNotCorrect));
            this.RuleFor(x => x.Username).NotEmpty().WithMessage(nameof(HfdCoreError.UserUsernameCannotBeEmpty));
            this.RuleFor(x => x.Username).NotEmpty().WithMessage(nameof(HfdCoreError.UserUsernameCannotBeEmpty));
            this.RuleFor(x => x.UniqueId).NotEmpty().WithMessage(nameof(HfdCoreError.UserUniqueIdCannotBeEmpty));
            this.RuleFor(x => x.FullName).NotEmpty().WithMessage(nameof(HfdCoreError.UserFullNameCannotBeEmpty));
        }
    }
}
