namespace Hfd.Base.Contract.Dto;

/// <summary>
///     Base model.
/// </summary>
public class BaseModel
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
	///     Last user updated record.
	/// </summary>
	public long? CreatedBy { get; set; }

	/// <summary>
	///     Update record date, time.
	/// </summary>
	public DateTime? UpdatedAt { get; set; }

	/// <summary>
	///     Last user updated record.
	/// </summary>
	public long? UpdatedBy { get; set; }

	/// <summary>
	///     Deleted flag.
	/// </summary>
	public bool Deleted { get; set; }
}