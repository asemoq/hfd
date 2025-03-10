using Hfd.Common.ApiClient.Model;
using Hfd.Common.Contract.Model;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Get all nurses.
/// </summary>
public static class NurseGetAll
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : SkipTakeRq, IRequest<PageList<NurseModel>>
	{
	}

	/// <inheritdoc />
	public sealed class Handler : IRequestHandler<Request, PageList<NurseModel>>
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
		public async Task<PageList<NurseModel>> Handle(Request rq, CancellationToken token)
		{
			var service = _context.Services.GetRequiredService<IDbStorage>().Get<INurseService>();
			return _context.GetMapper().Map<INurseFull, NurseModel>(await service.GetAll(rq));
		}

		/// <inheritdoc />
		public sealed class Validator : AbstractValidator<Request>
		{
		}
	}
}