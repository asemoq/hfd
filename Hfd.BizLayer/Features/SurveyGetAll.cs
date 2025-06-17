using Hfd.Common.Models;

namespace Hfd.BizLayer.Features;

public static class SurveyGetAll
{
    /// <inheritdoc cref="SkipTakeRq" />
    public sealed class Request : SkipTakeRq, IRequest<IPageList<ISurveyFull>>
    {
        /// <summary>
        ///     Id пациента.
        /// </summary>
        public long? PatientId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IPageList<ISurveyFull>>
    {
        /// <inheritdoc />
        public async Task<IPageList<ISurveyFull>> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<ISurveyService>();
            return await service.GetAll(request, request.PatientId);
        }
    }

    /// <inheritdoc />
    public sealed class Auditor : AbstractValidator<Request>
    {
    }
}
