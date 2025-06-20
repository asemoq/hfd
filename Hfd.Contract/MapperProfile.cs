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
        this.CreateMap<INurseFull, NurseModel>().IncludeBase<IPersonInfo, PersonModel>();
        this.CreateMap<IPatientFull, PatientModel>().IncludeBase<IPersonInfo, PersonModel>();
        this.CreateMap<IPatientFull, StaffContact>().ConvertUsing<StaffContactTypeConverter>();
        this.CreateMap<IPlotFull, PlotModel>();
        this.CreateMap<ISurveyFull, SurveyModel>();
    }
}
