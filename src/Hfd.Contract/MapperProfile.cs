using AutoMapper;
using Hfd.Contract.Dto;
using Hfd.Contract.Model;

namespace Hfd.Contract;

/// <summary>
///     Automapper setup.
/// </summary>
public class MapperProfile : Profile
{
	public MapperProfile()
	{
		CreateMap<INurseFull, NurseModel>();
		CreateMap<IPatientFull, PatientModel>();
		CreateMap<ISurveyFull, SurveyModel>().ForMember(dest => dest.PatientName,
			opt => opt.MapFrom(src =>
				src.PatientModel.User.FullName));
	}
}