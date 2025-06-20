using Hfd.Common.Models;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Get all plots.
/// </summary>
public static class PlotGetAll
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : SkipTakeRq, IRequest<IPageList<IPlotFull>>
    {
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IPageList<IPlotFull>>
    {
        /// <inheritdoc />
        public async Task<IPageList<IPlotFull>> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IPlotService>();
            return await service.GetAll(request);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
        }
    }
}
