#nullable disable
using AutoMapper;
using Hfd.Common.Contract.Helpers;
using Hfd.Common.Contract.Services;
using Hfd.Common.Models;
using Hfd.Contract.Services;
using Hfd.Models;

namespace Hfd.Contract;

/// <inheritdoc/>
public partial class MapperProfile : Profile
{
    internal sealed class UserRoleAction(IDbStorage storage) : IMappingAction<UserFull, UserModel>
    {
        public void Process(UserFull src, UserModel dst, ResolutionContext context)
        {
            AsyncHelper.RunSync(async () =>
            {
                switch (src.DefaultRole)
                {
                    case UserRole.Nurse:
                    {
                        var nurse = await storage.GetService<INurseService>().GetByUserId(src.Id);
                        dst.Nurse = context.Mapper.Map<NurseModel>(nurse);
                        break;
                    }

                    case UserRole.Patient:
                    {
                        var patient = await storage.GetService<IPatientService>().GetByUserId(src.Id);
                        dst.Patient = context.Mapper.Map<PatientModel>(patient);
                        break;
                    }

                    case UserRole.Administrator:
                    case UserRole.Psychologist:
                    case UserRole.SocialWorker:
                    default:
                        break;
                }
            });
        }
    }
}
