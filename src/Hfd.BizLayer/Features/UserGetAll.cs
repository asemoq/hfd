using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Model;
using Hfd.Base.Contract.Services;
using Hfd.Contract.Services;

namespace Hfd.BizLayer.Features;

public static class UserGetAll
{
	/// <inheritdoc cref="SkipTakeRq" />
	public sealed class Request : SkipTakeRq, IRequest<PageList<UserFull>>
	{
	}

	/// <inheritdoc />
	public sealed class Handler : IRequestHandler<Request, PageList<UserFull>>
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
		public async Task<PageList<UserFull>> Handle(Request rq, CancellationToken token)
		{
			var service = _context.Services.GetRequiredService<IDbStorage>().Get<IUserLoginDbService>();
			return _context.GetMapper().Map<IUserFull, UserFull>(await service.GetAll(rq));
		}
	}

	/// <inheritdoc />
	public sealed class Validator : AbstractValidator<Request>
	{
	}
}