using FluentValidation.Results;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Helpers;
using Hfd.Base.Contract.Model;
using Hfd.Base.Contract.Services;
using Microsoft.Extensions.Logging;

namespace Hfd.Base.BizLayer.Features;

public static class UserDelete
{
	/// <summary>
	///     Request.
	/// </summary>
	public sealed class Request : IRequest, IRequest<Unit>
	{
		/// <summary>
		///     Id пользователя.
		/// </summary>
		internal long Id { get; set; }

		/// <summary>
		///     Пользователь.
		/// </summary>
		internal IUserFull User { get; set; }

		public static Request With(long id)
		{
			return new Request() { Id = id };
		}
	}

	/// <inheritdoc />
	public sealed class Handler : IRequestHandler<Request, Unit>
	{
		public readonly IBizContext _context;

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
				.Delete(rq.Id);

			return Unit.Value;
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