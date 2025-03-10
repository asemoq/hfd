namespace Hfd.BizLayer.Features;

/// <summary>
///     Get patient by user id.
/// </summary>
public static class PatientGetByUserId
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : IRequest<PatientModel>
	{
		public long UserId { get; set; }
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
			return _context.GetMapper().Map<IPatientFull, PatientModel>(await service.GetByUserId(rq.UserId));
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