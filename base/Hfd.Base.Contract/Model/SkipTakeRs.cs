namespace Hfd.Base.Contract.Model;

/// <summary>
///     Результат запроса по фильтру <see cref="SkipTakeRq" />.
/// </summary>
public class SkipTakeRs
{
	/// <summary>
	///     Общее количество данных, соответствующих параметрам запроса.
	/// </summary>
	public int Total { get; set; } = 0;

	/// <summary>
	///     Индекс первой записи, совпадает с параметром <i>skip</i> запроса.
	/// </summary>
	public int First { get; set; } = 0;
}