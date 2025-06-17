using Hfd.Common.Models;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Get all patients.
/// </summary>
public static class PatientGetAll
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : SkipTakeRq, IRequest<IPageList<IPatientFull>>
    {
        public long? NurseId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler(IDbStorage storage) : IRequestHandler<Request, IPageList<IPatientFull>>
    {
        /// <inheritdoc />
        public async Task<IPageList<IPatientFull>> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = storage.GetService<IPatientService>();
            return await service.GetAll(request, request.NurseId);
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
        }
    }
}
