using Hfd.Common.Models;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Get all nurses.
/// </summary>
public static class NurseGetAll
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : SkipTakeRq, IRequest<IPageList<INurseFull>>
    {
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IPageList<INurseFull>>
    {
        /// <inheritdoc />
        public async Task<IPageList<INurseFull>> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<INurseService>();
            return await service.GetAll(request);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
        }
    }
}
