using System.ComponentModel;

namespace Hfd.Base.Contract.Model;

/// <summary>
///     Интерфейс фильтра для организации постраничного доступа к данным.
/// </summary>
public interface ISkipTakeRq
{
	/// <summary>
	///     Количество данных, которые надо пропустить. Значение должно быть неотрицательным.
	/// </summary>
	int Skip { get; }

	/// <summary>
	///     Количество данных, которые надо вернуть. Значение должно быть неотрицательным.
	///     Если задано значение 0, то будут возвращены все значения.
	/// </summary>
	int Take { get; }
}

/// <summary>
///     Фильтр для организации постраничного доступа к данным.
/// </summary>
public class SkipTakeRq : ISkipTakeRq
{
	/// <inheritdoc />
	[DefaultValue(0)]
	public int Skip { get; set; }

	/// <inheritdoc />
	[DefaultValue(100)]
	public int Take { get; set; } = 100;
}