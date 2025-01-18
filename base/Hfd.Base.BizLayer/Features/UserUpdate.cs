using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Helpers;
using Hfd.Base.Contract.Internals;
using Hfd.Base.Contract.Model;
using Hfd.Base.Contract.Services;

namespace Hfd.Base.BizLayer.Features;

public static class UserUpdate
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : UserInfo, IRequest<UserFull>
	{
		/// <summary>
		///     Идентификатор пользователя.
		/// </summary>
		public long Id { get; set; }
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
			var user = await service.Update(rq.Id, rq);
			return _context.GetMapper().Map<IUserFull, UserFull>(user);
		}
	}

	/// <inheritdoc />
	public sealed class Validator : AbstractValidator<Request>
	{
		public Validator()
		{
			RuleFor(x => x.Id).GreaterThan(0)
				.WithMessage(nameof(HfdBaseError.User_Id_Not_Correct));
			RuleFor(x => x.Username).NotEmpty()
				.WithMessage(nameof(HfdBaseError.User_Username_Cannot_Be_Empty));
			RuleFor(x => x.Usertype).IsInEnum()
				.WithMessage(nameof(HfdBaseError.User_Usertype_Not_Correct));
			RuleFor(x => x.Language).IsInEnum()
				.WithMessage(nameof(HfdBaseError.User_Language_Not_Correct));
			RuleFor(x => x.Username).NotEmpty()
				.WithMessage(nameof(HfdBaseError.User_Username_Cannot_Be_Empty));
			RuleFor(x => x.UniqueId).NotEmpty()
				.WithMessage(nameof(HfdBaseError.User_UniqueId_Cannot_Be_Empty));
			RuleFor(x => x.FullName).NotEmpty()
				.WithMessage(nameof(HfdBaseError.User_FullName_Cannot_Be_Empty));
		}
	}
}