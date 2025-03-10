using Hfd.Common.Contract.Model;

namespace Hfd.DbLayer.Internals;

/// <summary>
///     Base entity.
/// </summary>
public class HfdEntityBase : IBaseInfo
{
	/// <summary>
	///     Identifier.
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	///     Create record date, time.
	/// </summary>
	public DateTime CreatedAt { get; set; }

	/// <summary>
	///     Update record date, time.
	/// </summary>
	public DateTime? UpdatedAt { get; set; }
}