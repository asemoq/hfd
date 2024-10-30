namespace Hfd.Base.Contract.Services;

/// <summary>
///     Базовый интерфейс для сервиса данных.
/// </summary>
public interface IDbService
{
	/// <summary>
	///     Репозиторий приложения.
	/// </summary>
	IDbStorage Storage { get; }
}