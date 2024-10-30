using Hfd.Base.Contract.Services;
using Hfd.Contract.Dto;
using Hfd.Contract.Model;
using Hfd.Contract.Services;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Delete nurse.
/// </summary>
public static class NurseDelete
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : IRequest<NurseModel>
	{
		public long NurseId { get; set; }
	}

	/// <inheritdoc />
	public sealed class Handler : IRequestHandler<Request, NurseModel>
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
		public async Task<NurseModel> Handle(Request rq, CancellationToken token)
		{
			var service = _context.Services.GetRequiredService<IDbStorage>().Get<INurseService>();
			return _context.GetMapper()
				.Map<INurseModel, NurseModel>(await service.Delete(rq.NurseId));
		}

		/// <inheritdoc />
		public sealed class Validator : AbstractValidator<Request>
		{
		}
	}
}