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
			_context = context;
		}

		/// <inheritdoc />
		public async Task<PageList<PatientModel>> Handle(Request rq, CancellationToken token)
		{
			var service = _context.Services.GetRequiredService<IDbStorage>().Get<IPatientService>();
			return _context.GetMapper().Map<IPatientFull, PatientModel>(await service.GetAll(rq, rq.NurseId));
		}

		/// <inheritdoc />
		public sealed class Validator : AbstractValidator<Request>
		{
		}
	}
}