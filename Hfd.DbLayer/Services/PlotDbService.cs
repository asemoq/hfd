using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Models;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Contract.Services;

namespace Hfd.DbLayer.Services;

public class PlotDbService(HfdDbStorage storage, ILogger<PlotDbService> log)
    : HfdDbService(storage, log), IPlotService
{
    public async Task<IPlotFull> Add(IPlotInfo plotInfo)
    {
        return await this.Execute(async model =>
        {
            await using var trans = await model.Database.BeginTransactionAsync();
            try
            {
                var plot = new PlotEntity
                {
                    PlotNumber = plotInfo.PlotNumber, Area = plotInfo.Area, CreatedAt = DateTime.UtcNow,
                };
                await model.Plots.AddAsync(plot);
                await model.SaveChangesAsync();
                await trans.CommitAsync();
                return plot;
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        });
    }

    public async Task<IPlotFull> Edit(long plotId, IPlotInfo plotInfo)
    {
        return await this.Execute(async model =>
        {
            await using var trans = await model.Database.BeginTransactionAsync();
            try
            {
                var plot = await model.Plots.SingleOrDefaultAsync(x => x.Id == plotId);
                if (plot == null)
                {
                    throw new HfdException(nameof(HfdError.HFDPlotNotExists));
                }

                plot.PlotNumber = plotInfo.PlotNumber;
                plot.Area = plotInfo.Area;
                plot.UpdatedAt = DateTime.UtcNow;

                await model.SaveChangesAsync();
                await trans.CommitAsync();
                return plot;
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        });
    }

    public async Task<IPageList<IPlotFull>> GetAll(ISkipTakeRq request)
    {
        return await this.Execute(async model =>
        {
            var total = await model.Plots.CountAsync();
            var skip = Math.Min(total, request.Skip);
            var take = Math.Min(total - request.Skip, request.Take == 0 ? total : request.Take);
            return new PageList<IPlotFull>
            {
                Total = total,
                First = skip,
                Items = take > 0
                    ? await model.Plots.AsNoTracking().Skip(skip).Take(take).ToListAsync<IPlotFull>()
                    : new List<IPlotFull>(),
            };
        });
    }

    public async Task<IPlotFull> GetById(long plotId, bool includeDeps = false)
    {
        return await this.Execute(async model =>
        {
            return includeDeps
                ? await model.Plots.Include(x => x.NurseEntities)
                      .Include(x => x.PatientEntities)
                      .Include(x => x.PsychologistEntities)
                      .Include(x => x.SocialWorkerEntities)
                      .SingleOrDefaultAsync(x => x.Id == plotId)
                  ?? throw new HfdException(nameof(HfdError.HFDPlotNotFound))
                : await model.Plots.SingleOrDefaultAsync(x => x.Id == plotId)
                  ?? throw new HfdException(nameof(HfdError.HFDPlotNotFound));
        });
    }

    public async Task<IPlotFull> Delete(long plotId)
    {
        return await this.Execute(async model =>
        {
            await using var trans = await model.Database.BeginTransactionAsync();
            try
            {
                var plot = await model.Plots.SingleOrDefaultAsync(x => x.Id == plotId);
                if (plot == null)
                {
                    throw new HfdException(nameof(HfdError.HFDPlotNotExists));
                }

                model.Remove(plot);
                await model.SaveChangesAsync();
                await trans.CommitAsync();
                return plot;
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        });
    }
}
