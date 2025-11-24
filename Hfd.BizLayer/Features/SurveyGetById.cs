namespace Hfd.BizLayer.Features;

public static class SurveyGetById
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : IRequest<ISurveyFull>
    {
        /// <summary>
        ///     Id опроса.
        /// </summary>
        public long SurveyId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, ISurveyFull>
    {
        /// <inheritdoc />
        public async Task<ISurveyFull> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<ISurveyService>();
            return await service.GetById(request.SurveyId);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                this.RuleFor(x => x.SurveyId).GreaterThan(0).WithMessage(nameof(HfdError.SurveyIdIsNotCorrect));
            }
        }
    }
}
