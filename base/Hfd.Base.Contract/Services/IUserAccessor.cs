using Hfd.Base.Contract.Dto;

namespace Hfd.Base.Contract.Services;

public interface IUserAccessor
{
	/// <summary>
	///     Сервисы контекста.
	/// </summary>
	IServiceProvider Services { get; }

	/// <summary>
	///     Идентификатор.
	/// </summary>
	string ScopeId { get; }

	/// <summary>
	///     Учетная запись.
	/// </summary>
	UserFull UserFull { get; }

	/// <summary>
	///     Токен отмены.
	/// </summary>
	CancellationToken Aborted { get; }
}