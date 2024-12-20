using FluentValidation.Results;
using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Helpers;
using Hfd.Base.Contract.Model;
using Hfd.Base.Contract.Services;
using Microsoft.Extensions.Logging;

namespace Hfd.Base.BizLayer.Features;

public static class UserBlock
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : IRequest<UserFull>
	{
		/// <summary>
		///     Оставить учетную запись заблокированной.
		/// </summary>
		public bool Block { get; set; }

		/// <summary>
		///     Причина блокировки.
		/// </summary>
		public string BlockedReason { get; set; }

		/// <summary>
		///     Id пользователя.
		/// </summary>
		internal long Id { get; set; }

		/// <summary>
		///     Пользователь.
		/// </summary>
		internal IUserFull User { get; set; }

		public Request With(long id)
		{
			return (this, Id = id).Item1;
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
			var user = await service.Block(rq.Id, rq.Block, rq.BlockedReason);
			return _context.GetMapper().Map<IUserFull, UserFull>(user);
		}
	}

	/// <inheritdoc />
	public sealed class Validator : AbstractValidator<Request>
	{
		private readonly IDbStorage _dataStorage;
		private readonly ILogger<Validator> _log;

		public Validator(IDbStorage dataStorage, ILogger<Validator> log)
		{
			_dataStorage = dataStorage;
			_log = log;

			RuleFor(x => x.Id).Must((cm, id) => cm.User != null)
				.WithMessage("Не найдена учетная запись.");

			RuleFor(x => x.BlockedReason).NotEmpty().When(x => x.Block)
				.WithMessage(nameof(HfdBaseError.User_BlockedReason_Cannot_Be_Empty));
		}

		protected override bool PreValidate(ValidationContext<Request> context, ValidationResult result)
		{
			try
			{
				var id = context.InstanceToValidate?.Id;
				if (id != null)
				{
					var user = AsyncHelper.RunSync(async () =>
						await _dataStorage.Get<IUserLoginDbService>().GetOne(id.Value));
					context.InstanceToValidate.User = user;
				}
			}
			catch (Exception ex)
			{
				_log.LogError(ex, "Не найдена учетная запись.");
			}

			return base.PreValidate(context, result);
		}
	}
}