#nullable disable
using AutoMapper;
using Hfd.Contract.Model;
using Hfd.Models;

namespace Hfd.Contract;

/// <inheritdoc/>
public partial class MapperProfile : Profile
{
    internal class StaffContactTypeConverter : ITypeConverter<IPatientFull, StaffContact>
    {
        public StaffContact Convert(IPatientFull source, StaffContact destination, ResolutionContext context)
        {
            var nurse = context.Mapper.Map<INurseFull, NurseModel>(source.Nurse);
            destination = new StaffContact() { Nurse = nurse.User.PhoneNumber };
            return destination;
        }
    }
}
