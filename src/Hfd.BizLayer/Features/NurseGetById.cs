using Hfd.Base.Contract.Services;
using Hfd.Contract.Dto;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Contract.Services;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Get nurse by id.
/// </summary>
public static class NurseGetById
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
				.Map<INurseModel, NurseModel>(await service.GetById(rq.NurseId));
		}

		/// <inheritdoc />
		public sealed class Validator : AbstractValidator<Request>
		{
			public Validator()
			{
				RuleFor(x => x.NurseId).GreaterThan(0)
					.WithMessage(nameof(HfdError.HFD_Nurse_Id_Is_Not_Correct));
			}
		}
	}
}