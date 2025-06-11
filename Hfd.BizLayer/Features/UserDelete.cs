using Hfd.Common.Contract.Model;

namespace Hfd.BizLayer.Features;

public static class UserDelete
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<Unit>
    {
        /// <summary>
        ///     Id пользователя.
        /// </summary>
        internal long Id { get; set; }

        /// <summary>
        ///     Пользователь.
        /// </summary>
        internal IUserFull User { get; set; }

        public static Request With(long id)
        {
            return new Request() { Id = id };
        }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, Unit>
    {
        /// <inheritdoc />
        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IUserLoginDbService>();
            await service.Delete(request.Id);
            return Unit.Value;
        }
    }

    /// <inheritdoc />
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator(IDbStorage storage)
        {
            this.RuleFor(x => x.Id)
                .CustomAsync(async (id, context, cancellationToken) =>
                {
                    var user = await storage.GetService<IUserLoginDbService>().GetOne(id);
                    context.InstanceToValidate.User = user;
                })
                .Must((cm, id) => cm.User != null!)
                .WithMessage("Не найдена учетная запись.");
        }
    }
}
