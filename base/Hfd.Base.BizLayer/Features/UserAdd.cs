using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Internals;
using Hfd.Base.Contract.Model;
using Hfd.Base.Contract.Services;
using static Hfd.Base.Contract.Helpers.FieldFormatHelper;

namespace Hfd.Base.BizLayer.Features;

public static class UserAdd
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : UserInfo, IRequest<UserFull>
	{
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
		public async Task<UserFull> Handle(Request rq, CancellationToken cancellationToken)
		{
			var service = _context.Services.GetRequiredService<IDbStorage>().Get<IUserLoginDbService>();
			var user = await service.Create(rq.Usertype, rq, rq.Password.Hash());
			return _context.GetMapper().Map<IUserFull, UserFull>(user);
		}
	}

	/// <inheritdoc />
	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.Usertype).IsInEnum()
				.WithMessage(nameof(HfdBaseError.User_Usertype_Not_Correct));
			RuleFor(x => x.Language).IsInEnum()
				.WithMessage(nameof(HfdBaseError.User_Language_Not_Correct));
			RuleFor(x => x.Password).NotEmpty()
				.WithMessage(nameof(HfdBaseError.User_Password_Cannot_Be_Empty));
			RuleFor(x => x.Username).NotEmpty()
				.WithMessage(nameof(HfdBaseError.User_Username_Cannot_Be_Empty));
			RuleFor(x => x.UniqueId).Must(IsValidIin)
				.WithMessage(nameof(HfdBaseError.User_UniqueId_Cannot_Is_Not_Valid));
			RuleFor(x => x.FullName).NotEmpty()
				.WithMessage(nameof(HfdBaseError.User_FullName_Cannot_Be_Empty));
		}
	}
}