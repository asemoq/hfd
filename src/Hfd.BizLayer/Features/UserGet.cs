using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Model;
using Hfd.Base.Contract.Services;
using Hfd.Contract.Services;

namespace Hfd.BizLayer.Features;

public static class UserGet
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : IRequest<UserFull>
	{
		/// <summary>
		///     Id пользователя.
		/// </summary>
		internal long Id { get; set; }

		public static Request With(long id)
		{
			var rq = new Request { Id = id };
			return rq;
		}
	}

	/// <inheritdoc />
	public sealed class Handler : IRequestHandler<Request, UserFull>
	{
		private readonly IBizContext _context;

		public Handler(IBizContext context)
		{
			_context = context;
		}

		/// <inheritdoc />
		public async Task<UserFull> Handle(Request rq, CancellationToken token)
		{
			var service = _context.Services.GetRequiredService<IDbStorage>().Get<IUserLoginDbService>();
			return _context.GetMapper().Map<IUserFull, UserFull>(await service.GetOne(rq.Id));
		}
	}
}