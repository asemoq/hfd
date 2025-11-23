namespace Hfd.BizLayer.Features;

/// <summary>
///     Add plot.
/// </summary>
public static class PlotAdd
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IPlotInfo, IRequest<IPlotFull>
    {
        /// <inheritdoc />
        public int Number { get; set; }

        /// <inheritdoc />
        public string Area { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IPlotFull>
    {
        /// <inheritdoc />
        public async Task<IPlotFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IPlotService>();
            return await service.Add(request);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
            }
        }
    }
}
