namespace Hfd.BizLayer.Features;

public static class SurveyNecessityByPatientId
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : IRequest<bool>
	{
		/// <summary>
		///     Id пациента.
		/// </summary>
		public long PatientId { get; set; }
	}

	/// <inheritdoc />
	public sealed class Handler : IRequestHandler<Request, bool>
	{
		public readonly IBizContext _context;

		public Handler(IBizContext context)
		{
			_context = context;
		}

		/// <inheritdoc />
		public async Task<bool> Handle(Request rq, CancellationToken token)
		{
			var service = _context.Services.GetRequiredService<IDbStorage>().Get<ISurveyService>();
			return await service.NecessityByPatientId(rq.PatientId);
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