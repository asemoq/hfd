using Hfd.Base.Contract.Services;
using Hfd.Contract.Dto;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Contract.Services;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Add patient.
/// </summary>
public static class PatientAdd
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : IPatientInfo, IRequest<PatientModel>
	{
		/// <inheritdoc />
		public long UserId { get; set; }

		/// <inheritdoc />
		public string Scale { get; set; }

		/// <inheritdoc />
		public string Imt { get; set; }

		/// <inheritdoc />
		public int Klas { get; set; }

		/// <inheritdoc />
		public int PlotNumber { get; set; }

		/// <inheritdoc />
		public bool Dead { get; set; }

		/// <summary>
		///     Карточка пациента.
		/// </summary>
		public PatientCardInfo PatientCard { get; set; }
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
			return _context.GetMapper().Map<IPatientModel, PatientModel>(await service.Add(rq, rq.PatientCard));
		}

		/// <inheritdoc />
		public sealed class Validator : AbstractValidator<Request>
		{
			public Validator()
			{
				RuleFor(x => x.UserId).GreaterThan(0)
					.WithMessage(nameof(HfdError.HFD_Patient_UserId_Is_Not_Correct));
			}
		}
	}
}