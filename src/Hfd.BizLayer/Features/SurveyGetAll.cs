using Hfd.Base.Contract.Model;
using Hfd.Base.Contract.Services;
using Hfd.Contract.Dto;
using Hfd.Contract.Model;
using Hfd.Contract.Services;

namespace Hfd.BizLayer.Features;

public static class SurveyGetAll
{
	/// <inheritdoc cref="SkipTakeRq" />
	public sealed class Request : SkipTakeRq, IRequest<PageList<SurveyModel>>
	{
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
			return _context.GetMapper().Map<ISurveyModel, SurveyModel>(await service.GetAll(rq, rq.PatientId));
		}
	}

	/// <inheritdoc />
	public sealed class Auditor : AbstractValidator<Request>
	{
	}
}