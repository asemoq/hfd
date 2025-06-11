using Hfd.Common.Contract.Model;

namespace Hfd.BizLayer.Features;

public static class UserBlock
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<IUserFull>
    {
        /// <summary>
        ///     Оставить учетную запись заблокированной.
        /// </summary>
        public bool Block { get; set; }

        /// <summary>
        ///     Id пользователя.
        /// </summary>
        internal long Id { get; set; }

        /// <summary>
        ///     Пользователь.
        /// </summary>
        internal IUserFull User { get; set; }

        public Request With(long id)
        {
            return (this, this.Id = id).Item1;
        }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IUserFull>
    {
        /// <inheritdoc />
        public async Task<IUserFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IUserLoginDbService>();
            return await service.Block(request.Id, request.Block);
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
