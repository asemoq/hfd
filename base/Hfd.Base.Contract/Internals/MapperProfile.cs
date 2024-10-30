using AutoMapper;
using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Model;

namespace Hfd.Base.Contract.Internals;

public class MapperProfile : Profile
{
	public MapperProfile()
	{
		CreateMap<IUserFull, UserFull>(MemberList.Destination);
	}
}