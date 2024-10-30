namespace Hfd.Base.Contract.Model;

/// <summary>
///     Интерфейс постраничного доступа к данным.
/// </summary>
/// <typeparam name="T">Тип элемента данных.</typeparam>
public interface IPageList<T>
{
	/// <summary>
	///     Количество записей.
	/// </summary>
	int Total { get; }

	/// <summary>
	///     Индекс первой записи.
	/// </summary>
	int First { get; }

	/// <summary>
	///     Список записей.
	/// </summary>
	IReadOnlyList<T> Items { get; }
}

/// <summary>
///     Список с параметром количества строк.
/// </summary>
/// <typeparam name="T">Тип элемента списка.</typeparam>
public class PageList<T> : SkipTakeRs, IPageList<T>
{
	/// <inheritdoc cref="IPageList{T}.Items" />
	public List<T> Items { get; set; }

	/// <inheritdoc />
	IReadOnlyList<T> IPageList<T>.Items => Items;
}