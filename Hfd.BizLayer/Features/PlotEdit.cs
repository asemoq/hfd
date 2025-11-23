namespace Hfd.BizLayer.Features;

/// <summary>
///     Edit plot.
/// </summary>
public static class PlotEdit
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IPlotInfo, IRequest<IPlotFull>
    {
        /// <summary>
        ///     Идентификатор.
        /// </summary>
        public long Id { get; set; }

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
            return await service.Edit(request.Id, request);
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
