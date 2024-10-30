using Hfd.Base.Contract.Services;
using Hfd.Contract.Dto;
using Hfd.Contract.Model;
using Hfd.Contract.Services;

namespace Hfd.BizLayer.Features;

public static class SurveyGetById
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : IRequest<SurveyModel>
	{
		/// <summary>
		///     Id пользователя.
		/// </summary>
		public long SurveyId { get; set; }
	}

	/// <inheritdoc />
	public sealed class Handler : IRequestHandler<Request, SurveyModel>
	{
		public readonly IBizContext _context;

		public Handler(IBizContext context)
		{
			_context = context;
		}

		/// <inheritdoc />
		public async Task<SurveyModel> Handle(Request rq, CancellationToken token)
		{
			var service = _context.Services.GetRequiredService<IDbStorage>().Get<ISurveyService>();
			return _context.GetMapper().Map<ISurveyModel, SurveyModel>(await service.GetById(rq.SurveyId));
		}
	}
}