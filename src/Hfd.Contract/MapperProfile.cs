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
		CreateMap<INurseModel, NurseModel>();
		CreateMap<IPatientModel, PatientModel>();
		CreateMap<IPatientCardModel, PatientCardModel>();
		CreateMap<ISurveyModel, SurveyModel>();
	}
}