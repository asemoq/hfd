using Hfd.Base.Contract.Services;
using Hfd.Contract.Dto;
using Hfd.Contract.Model;
using Hfd.Contract.Services;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Delete patient.
/// </summary>
public static class PatientDelete
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : IRequest<PatientModel>
	{
		public long PatientId { get; set; }
	}

	/// <inheritdoc />
	public sealed class Handler : IRequestHandler<Request, PatientModel>
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
		public async Task<PatientModel> Handle(Request rq, CancellationToken token)
		{
			var service = _context.Services.GetRequiredService<IDbStorage>().Get<IPatientService>();
			return _context.GetMapper()
				.Map<IPatientModel, PatientModel>(await service.Delete(rq.PatientId));
		}

		/// <inheritdoc />
		public sealed class Validator : AbstractValidator<Request>
		{
		}
	}
}