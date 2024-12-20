using Hfd.Base.Contract.Services;
using Hfd.Contract.Dto;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Contract.Services;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Edit survey.
/// </summary>
public static class SurveyEdit
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : ISurveyInfo, IRequest<SurveyModel>
	{
		/// <summary>
		///     Идентификатор.
		/// </summary>
		public long Id { get; set; }

		/// <inheritdoc />
		public DyspneaType Dyspnea { get; set; }

		/// <inheritdoc />
		public double AdValue { get; set; }

		/// <inheritdoc />
		public double WeightValue { get; set; }

		/// <inheritdoc />
		public EdemaType Edema { get; set; }

		/// <inheritdoc />
		public double HeartRateValue { get; set; }

		/// <inheritdoc />
		public PositionInBedType PositionInBed { get; set; }

		/// <inheritdoc />
		public long PatientId { get; set; }

		/// <inheritdoc />
		public DateTime Date { get; set; }
	}

	/// <inheritdoc />
	public sealed class Handler : IRequestHandler<Request, SurveyModel>
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
		public async Task<SurveyModel> Handle(Request rq, CancellationToken token)
		{
			var service = _context.Services.GetRequiredService<IDbStorage>().Get<ISurveyService>();
			return _context.GetMapper().Map<ISurveyModel, SurveyModel>(await service.Edit(rq.Id, rq));
		}

		/// <inheritdoc />
		public sealed class Validator : AbstractValidator<Request>
		{
			public Validator()
			{
				RuleFor(x => x.PatientId).GreaterThan(0)
					.WithMessage(nameof(HfdError.HFD_Survey_PatientId_Is_Not_Correct));
			}
		}
	}
}