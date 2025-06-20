using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Models;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Contract.Services;

namespace Hfd.DbLayer.Services;

public class NurseDbService(HfdDbStorage storage, ILogger<NurseDbService> log)
    : HfdDbService(storage, log), INurseService
{
    public async Task<INurseFull> Add(INurseInfo nurseInfo)
    {
        return await this.Execute(async model =>
        {
            await using var trans = await model.Database.BeginTransactionAsync();
            try
            {
                var nurse = new NurseEntity
                {
                    UserId = nurseInfo.UserId, PlotId = nurseInfo.PlotId, CreatedAt = DateTime.UtcNow,
                };
                await model.Nurses.AddAsync(nurse);
                await model.SaveChangesAsync();
                await trans.CommitAsync();
                return nurse;
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        });
    }

    public async Task<INurseFull> Edit(long nurseId, INurseInfo nurseInfo)
    {
        return await this.Execute(async model =>
        {
            await using var trans = await model.Database.BeginTransactionAsync();
            try
            {
                var nurse = await model.Nurses.SingleOrDefaultAsync(x => x.Id == nurseId);
                if (nurse == null)
                {
                    throw new HfdException(nameof(HfdError.HFDNurseNotExists));
                }

                nurse.UserId = nurseInfo.UserId;
                nurse.PlotId = nurseInfo.PlotId;
                nurse.UpdatedAt = DateTime.UtcNow;

                await model.SaveChangesAsync();
                await trans.CommitAsync();
                return nurse;
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        });
    }

    public async Task<IPageList<INurseFull>> GetAll(ISkipTakeRq request)
    {
        return await this.Execute(async model =>
        {
            var total = await model.Nurses.CountAsync();
            var skip = Math.Min(total, request.Skip);
            var take = Math.Min(total - request.Skip, request.Take == 0 ? total : request.Take);
            return new PageList<INurseFull>
            {
                Total = total,
                First = skip,
                Items = take > 0
                    ? await model.Nurses.AsNoTracking().Skip(skip).Take(take).ToListAsync<INurseFull>()
                    : new List<INurseFull>(),
            };
        });
    }

    public async Task<INurseFull> GetById(long nurseId)
    {
        return await this.Execute(async model => await model.Nurses.SingleOrDefaultAsync(x => x.Id == nurseId)
                                                 ?? throw new HfdException(nameof(HfdError.HFDNurseNotFound)));
    }

    public async Task<INurseFull> GetByUserId(string userId)
    {
        return await this.Execute(async model => await model.Nurses.SingleOrDefaultAsync(x => x.UserId == userId)
                                                 ?? throw new HfdException(nameof(HfdError.HFDNurseNotFound)));
    }

    public async Task<INurseFull> Delete(long nurseId)
    {
        return await this.Execute(async model =>
        {
            await using var trans = await model.Database.BeginTransactionAsync();
            try
            {
                var nurse = await model.Nurses.SingleOrDefaultAsync(x => x.Id == nurseId);
                if (nurse == null)
                {
                    throw new HfdException(nameof(HfdError.HFDNurseNotExists));
                }

                model.Remove(nurse);
                await model.SaveChangesAsync();
                await trans.CommitAsync();
                return nurse;
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        });
    }
}
