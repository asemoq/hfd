namespace Hfd.Base.Contract.Model;

/// <summary>
///     Base model.
/// </summary>
public interface IBaseInfo
{
	/// <summary>
	///     Identifier.
	/// </summary>
	long Id { get; }

	/// <summary>
	///     Create record date, time.
	/// </summary>
	DateTime CreatedAt { get; }

	/// <summary>
	///     Update record date, time.
	/// </summary>
	DateTime? UpdatedAt { get; }
}