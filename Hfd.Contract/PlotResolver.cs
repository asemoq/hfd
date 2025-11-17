#nullable disable
using AutoMapper;
using Hfd.Common.Contract.Helpers;
using Hfd.Common.Contract.Services;
using Hfd.Contract.Model;
using Hfd.Contract.Services;
using Hfd.Models;

namespace Hfd.Contract;

public class PlotResolver(IDbStorage storage) : IValueResolver<IMedPersonInfo, MedPersonModel, PlotModel>
{
    public PlotModel Resolve(IMedPersonInfo source, MedPersonModel destination, PlotModel destMember, ResolutionContext context)
    {
        return source == null
            ? null
            : AsyncHelper.RunSync(async () =>
            {
                if (source.PlotId == null) return null;
                var plot = await storage.GetService<IPlotService>().GetById(source.PlotId.Value);
                return context.Mapper.Map<PlotModel>(plot);
            });
    }
}
