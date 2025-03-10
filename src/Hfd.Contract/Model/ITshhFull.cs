using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
/// </summary>
public interface ITshhFull : IBaseInfo, ITshhInfo
{
}

public interface ITshhInfo
{
	/// <summary>
	/// </summary>
	long Klass { get; }

	/// <summary>
	/// </summary>
	long Distance { get; }

	/// <summary>
	/// </summary>
	long PatientId { get; }

	/// <summary>
	/// </summary>
	DateTime Date { get; }
}