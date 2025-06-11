using Hfd.Common.ApiClient.Model;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Contract.Internals;

namespace Hfd.BizLayer.Features;

public static class UserPasswordChange
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : ChangePasswordInfo, IRequest<Unit>
    {
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, Unit>
    {
        /// <inheritdoc />
        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IUserLoginDbService>();
            await service.ChangePassword(request.NewPassword.Hash());
            return Unit.Value;
        }
    }

    /// <inheritdoc />
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator(IBizContext context)
        {
            var context1 = context;

            this.RuleFor(cm => cm.OldPassword)
                .MustAsync(async (pw, token) =>
                {
                    var user = await context1.GetService<IDbStorage>()
                        .GetService<IUserLoginDbService>()
                        .GetOne(context1.UserFull.Id);

                    return pw.Verify(user.Password);
                });

            this.RuleFor(cm => cm.NewPassword)
                .NotEmpty()
                .Length(3, 64)
                .WithMessage(nameof(HfdCoreError.UserPasswordLenghtNotCorrect));
        }
    }
}
