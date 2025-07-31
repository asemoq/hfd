#nullable disable
using System.Linq;
using AutoMapper;
using Hfd.Common.Contract;
using Hfd.Common.Contract.Helpers;
using Hfd.Common.Contract.Services;
using Hfd.Contract.Model;
using Hfd.Contract.Services;
using Hfd.Models;

namespace Hfd.Contract;

/// <inheritdoc/>
public partial class MapperProfile : Profile
{
    internal class StaffContactTypeConverter(IDbStorage storage) : ITypeConverter<IPatientFull, StaffContact>
    {
        public StaffContact Convert(IPatientFull source, StaffContact destination, ResolutionContext context)
        {
            return source == null
                ? null
                : AsyncHelper.RunSync(async () =>
                {
                    if (source.PlotId != null)
                    {
                        var service = storage.GetService<IPlotService>();
                        var plot = await service.GetById(source.PlotId.Value, true);
                        var nurses = context.Mapper.Map<INurseFull, NurseModel>(plot.Nurses);
                        destination = new StaffContact()
                        {
                            Nurses = nurses.Select(x => FieldFormatHelper.GetFormattedPhoneNumber(x.User.PhoneNumber)).ToArray(),
                        };
                    }

                    return destination;
                });
        }
    }
}
