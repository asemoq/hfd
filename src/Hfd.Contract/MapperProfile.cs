using AutoMapper;
using Hfd.Base.Contract.Helpers;
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
		CreateMap<INurseFull, NurseModel>().ForMember(dest => dest.PhoneNumber,
			opt => opt.MapFrom(src =>
				FieldFormatHelper.GetFormattedPhoneNumber(src.PhoneNumber, true)));
		CreateMap<IPatientFull, PatientModel>().ForMember(dest => dest.PhoneNumber,
			opt => opt.MapFrom(src =>
				FieldFormatHelper.GetFormattedPhoneNumber(src.PhoneNumber, true)));
		CreateMap<ISurveyFull, SurveyModel>().ForMember(dest => dest.PatientName,
			opt => opt.MapFrom(src =>
				src.PatientModel.User.FullName));
	}
}