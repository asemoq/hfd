using AutoMapper;
using Hfd.Common.Contract.Model;
using Hfd.Common.Models;
using Hfd.Contract.Model;
using Hfd.Models;

namespace Hfd.Contract;

/// <summary>
///     Automapper setup.
/// </summary>
public partial class MapperProfile : Profile
{
    public MapperProfile()
    {
        this.CreateMap<IMedPersonInfo, MedPersonModel>()
            .IncludeBase<IPersonInfo, PersonModel>()
            .ForMember(dest => dest.Plot, opt => opt.MapFrom<PlotResolver>());

        this.CreateMap<INurseFull, NurseModel>().IncludeBase<IMedPersonInfo, MedPersonModel>();
        this.CreateMap<IPatientFull, PatientModel>().IncludeBase<IMedPersonInfo, MedPersonModel>();
        this.CreateMap<IPatientFull, StaffContact>().ConvertUsing<StaffContactTypeConverter>();
        this.CreateMap<IPlotFull, PlotModel>();
        this.CreateMap<ISurveyFull, SurveyModel>();
        this.CreateMap<UserFull, UserModel>().AfterMap<UserRoleAction>();
    }
}
