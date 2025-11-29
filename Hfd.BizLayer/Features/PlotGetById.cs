namespace Hfd.BizLayer.Features;

/// <summary>
///     Get plot by id.
/// </summary>
public static class PlotGetById
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<IPlotFull>
    {
        public long PlotId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IPlotFull>
    {
        /// <inheritdoc />
        public async Task<IPlotFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IPlotService>();
            return await service.GetById(request.PlotId);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.PlotId).GreaterThan(0).WithError(ErrorType.NotCorrect, HfdError.PlotId);
            }
        }
    }
}
