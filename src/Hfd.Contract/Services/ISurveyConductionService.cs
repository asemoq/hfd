using Hfd.Contract.Model;

namespace Hfd.Contract.Services;

/// <summary>
///     Сервис управления опросами.
/// </summary>
public interface ISurveyConductionService
{
	/// <summary>
	///     Провести опрос.
	/// </summary>
	Task Request(IPatientFull patient);
}