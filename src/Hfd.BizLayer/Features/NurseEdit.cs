using Hfd.Base.Contract.Services;
using Hfd.Contract.Dto;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Contract.Services;
using static Hfd.Base.Contract.Helpers.FieldFormatHelper;

namespace Hfd.BizLayer.Features;

/// <summary>
///     Edit nurse.
/// </summary>
public static class NurseEdit
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : INurseInfo, IRequest<NurseModel>
	{
		/// <summary>
		///     Идентификатор.
		/// </summary>
		public long Id { get; set; }

		/// <inheritdoc />
		public string FullName { get; set; }

		/// <inheritdoc />
		public string PhoneNumber { get; set; }

		/// <inheritdoc />
		public long? PlotNumber { get; set; }

		/// <inheritdoc />
		public long UserId { get; set; }
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
			return _context.GetMapper().Map<INurseFull, NurseModel>(await service.Edit(rq.Id, rq));
		}

		/// <inheritdoc />
		public sealed class Validator : AbstractValidator<Request>
		{
			public Validator()
			{
				RuleFor(x => x.UserId).GreaterThan(0)
					.WithMessage(nameof(HfdError.HFD_Nurse_UserId_Is_Not_Correct));
				RuleFor(x => x.PhoneNumber).Must(IsValidPhoneNumber)
					.WithMessage(nameof(HfdError.HFD_Nurse_PhoneNumber_Is_Not_Valid));
			}
		}
	}
}