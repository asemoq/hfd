using Hfd.Common.ApiClient.Model;
using Hfd.Common.Contract.Model;

namespace Hfd.BizLayer.Features;

public static class SurveyGetAll
{
	/// <inheritdoc cref="SkipTakeRq" />
	public sealed class Request : SkipTakeRq, IRequest<PageList<SurveyModel>>
	{
		/// <summary>
		///     Id пациента.
		/// </summary>
		public long? PatientId { get; set; }
	}

	/// <inheritdoc />
	public sealed class Handler : IRequestHandler<Request, PageList<SurveyModel>>
	{
		public readonly IBizContext _context;

		/// <summary>
		///     Creates an instance of <see cref="Handler" />.
		/// </summary>
		public Handler(IBizContext context)
		{
			_context = context;
		}

		/// <inheritdoc />
		public async Task<PageList<SurveyModel>> Handle(Request rq, CancellationToken token)
		{
			var service = _context.Services.GetRequiredService<IDbStorage>().Get<ISurveyService>();
			return _context.GetMapper().Map<ISurveyFull, SurveyModel>(await service.GetAll(rq, rq.PatientId));
		}
	}

	/// <inheritdoc />
	public sealed class Auditor : AbstractValidator<Request>
	{
	}
}