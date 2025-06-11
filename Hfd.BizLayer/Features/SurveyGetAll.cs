using Hfd.Common.ApiClient.Model;
using Hfd.Common.Contract.Model;

namespace Hfd.BizLayer.Features;

public static class SurveyGetAll
{
    /// <inheritdoc cref="SkipTakeRq" />
    public sealed class Request : SkipTakeRq, IRequest<PageList<SurveyModel>>
    {
        /// <summary>
        ///     Id пациента.
        /// </summary>
        public long? PatientId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler : IRequestHandler<Request, PageList<SurveyModel>>
    {
        public readonly IBizContext _context;

        /// <summary>
        ///     Creates an instance of <see cref="Handler" />.
        /// </summary>
        public Handler(IBizContext context)
        {
            this._context = context;
        }

        /// <inheritdoc />
        public async Task<PageList<SurveyModel>> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = this._context.Services.GetRequiredService<IDbStorage>().GetService<ISurveyService>();
            return this._context.GetMapper().Map<ISurveyFull, SurveyModel>(await service.GetAll(request, request.PatientId));
        }
    }

    /// <inheritdoc />
    public sealed class Auditor : AbstractValidator<Request>
    {
    }
}
