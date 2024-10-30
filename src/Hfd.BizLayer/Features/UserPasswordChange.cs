using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Internals;
using Hfd.Base.Contract.Services;
using Hfd.Contract.Services;
using Microsoft.Extensions.Logging;

namespace Hfd.BizLayer.Features;

public static class UserPasswordChange
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : ChangePasswordInfo, IRequest, IRequest<Unit>
	{
	}

	/// <inheritdoc />
	public sealed class Handler : IRequestHandler<Request, Unit>
	{
		private readonly IBizContext _context;

		public Handler(IBizContext context)
		{
			_context = context;
		}

		/// <inheritdoc />
		public async Task<Unit> Handle(Request rq, CancellationToken token)
		{
			await _context.Services
				.GetRequiredService<IDbStorage>()
				.Get<IUserLoginDbService>()
				.ChangePassword(_context.UserFull.Id, rq.NewPassword.Hash());

			return Unit.Value;
		}
	}

	/// <inheritdoc />
	public sealed class Validator : AbstractValidator<Request>
	{
		private readonly IBizContext _context;
		private readonly ILogger<Validator> _log;

		public Validator(IBizContext context, ILogger<Validator> log)
		{
			_context = context;
			_log = log;

			RuleFor(cm => cm.OldPassword)
				.MustAsync(async (pw, token) =>
				{
					var user = await _context
						.GetService<IDbStorage>()
						.Get<IUserLoginDbService>()
						.GetOne(_context.UserFull.Id);

					return pw.Verify(user.Password);
				});

			RuleFor(cm => cm.NewPassword)
				.NotEmpty()
				.Length(3, 64)
				.WithMessage(nameof(HfdBaseError.User_Password_Lenght_Not_Correct));
		}
	}
}