using Hfd.Common.ApiClient.Model;
using Hfd.Common.Contract.Model;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Get all patients.
/// </summary>
public static class PatientGetAll
{
    /// <summary>
    ///     Request.
    /// </summary>
    public sealed class Request : SkipTakeRq, IRequest<PageList<PatientModel>>
    {
        public long? NurseId { get; set; }
    }

    /// <inheritdoc />
    public sealed class Handler : IRequestHandler<Request, PageList<PatientModel>>
    {
        private readonly IBizContext _context;

        /// <summary>
        ///     Creates an instance of <see cref="Handler" />.
        /// </summary>
        public Handler(IBizContext context)
        {
            this._context = context;
        }

        /// <inheritdoc />
        public async Task<PageList<PatientModel>> Handle(Request request, CancellationToken cancellationToken)
        {
            var service = this._context.Services.GetRequiredService<IDbStorage>().GetService<IPatientService>();
            return this._context.GetMapper().Map<IPatientFull, PatientModel>(await service.GetAll(request, request.NurseId));
        }

        /// <inheritdoc />
        public sealed class Validator : AbstractValidator<Request>
        {
        }
    }
}
