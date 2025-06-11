using Hfd.Common.ApiClient.Model;
using Hfd.Common.Contract.Model;

namespace Hfd.BizLayer.Features;

public static class UserGetAll
{
    /// <inheritdoc cref="SkipTakeRq" />
    public sealed class Request : UserFilter, IRequest<IPageList<IUserFull>>
    {
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IPageList<IUserFull>>
    {
        /// <inheritdoc />
        public async Task<IPageList<IUserFull>> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IUserLoginDbService>();
            return await service.GetAll(request);
        }
    }

    /// <inheritdoc />
    public sealed class Validator : AbstractValidator<Request>
    {
    }
}
