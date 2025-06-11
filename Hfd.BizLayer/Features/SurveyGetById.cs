namespace Hfd.BizLayer.Features;

public static class SurveyGetById
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<SurveyModel>
    {
        /// <summary>
        ///     Id опроса.
        /// </summary>
        public long SurveyId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler : IRequestHandler<Request, SurveyModel>
    {
        public readonly IBizContext _context;

        public Handler(IBizContext context)
        {
            this._context = context;
        }

        /// <inheritdoc />
        public async Task<SurveyModel> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = this._context.Services.GetRequiredService<IDbStorage>().GetService<ISurveyService>();
            return this._context.GetMapper().Map<ISurveyFull, SurveyModel>(await service.GetById(request.SurveyId));
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.SurveyId).GreaterThan(0).WithMessage(nameof(HfdError.HFDSurveyIdIsNotCorrect));
            }
        }
    }
}
