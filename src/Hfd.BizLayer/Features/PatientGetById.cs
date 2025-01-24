using Hfd.Base.Contract.Services;
using Hfd.Contract.Dto;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Contract.Services;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Get patient by id.
/// </summary>
public static class PatientGetById
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
				.Map<IPatientFull, PatientModel>(await service.GetById(rq.PatientId));
		}

		/// <inheritdoc />
		public sealed class Validator : AbstractValidator<Request>
		{
			public Validator()
			{
				RuleFor(x => x.PatientId).GreaterThan(0)
					.WithMessage(nameof(HfdError.HFD_Patient_Id_Is_Not_Correct));
			}
		}
	}
}