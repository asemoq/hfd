using AutoMapper;
using Hfd.Common.Contract.Helpers;
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
        this.CreateMap<INurseFull, NurseModel>()
            .ForMember(
                dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => FieldFormatHelper.GetFormattedPhoneNumber(src.PhoneNumber, true)));
        this.CreateMap<IPatientFull, PatientModel>()
            .ForMember(
                dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => FieldFormatHelper.GetFormattedPhoneNumber(src.PhoneNumber!, true)));
        this.CreateMap<ISurveyFull, SurveyModel>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.PatientModel.FullName));
    }
}
