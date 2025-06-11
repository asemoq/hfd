using Hfd.Common.Contract.Model;

namespace Hfd.BizLayer.Features;

public static class UserGet
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<IUserFull>
    {
        /// <summary>
        ///     Id пользователя.
        /// </summary>
        internal long Id { get; set; }

        public static Request With(long id)
        {
            var rq = new Request { Id = id };
            return rq;
        }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IUserFull>
    {
        /// <inheritdoc />
        public async Task<IUserFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IUserLoginDbService>();
            return await service.GetOne(request.Id);
        }
    }
}
