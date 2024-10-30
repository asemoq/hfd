using AutoMapper;

namespace Hfd.Base.Contract.Model;

/// <summary>
///     Automapper setup.
/// </summary>
public static class MapperExtensions
{
	public static PageList<Q> Map<T, Q>(this IMapper mapper, IPageList<T> pageList)
	{
		return new PageList<Q>
		{
			Total = pageList.Total,
			First = pageList.First,
			Items = pageList.Items?.Select(t => mapper.Map<Q>(t)).ToList()
		};
	}
}