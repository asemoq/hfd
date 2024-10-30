using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hfd.Base.Contract.Model;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Contract.Services;
using Hfd.DbLayer.Internals;
using Microsoft.EntityFrameworkCore;

namespace Hfd.DbLayer.Services;

public class NurseDbService : HfdDbService, INurseService
{
	/// <summary>
	///     Constructor of Nurse service class.
	/// </summary>
	/// <param name="storage"></param>
	/// <param name="log"></param>
	public NurseDbService(HfdDbStorage storage, ILogger<NurseDbService> log)
		: base(storage, log)
	{
	}

	public async Task<INurseModel> Add(INurseInfo nurseInfo)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var nurse = new NurseEntity
				{
					UserId = nurseInfo.UserId,
					PhoneNumber = nurseInfo.PhoneNumber,
					PlotNumber = nurseInfo.PlotNumber,
					CreatedAt = DateTime.UtcNow
				};
				await model.Nurses.AddAsync(nurse);
				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return nurse;
			}
			catch (Exception ex)
			{
				await trans.RollbackAsync();
				throw new Exception(ex.Message);
			}
		});
	}

	public async Task<INurseModel> Edit(long nurseId, INurseInfo nurseInfo)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var nurse = await model.Nurses.SingleOrDefaultAsync(x => x.Id == nurseId);
				if (nurse == null)
					throw new Exception(nameof(HfdError.HFD_Nurse_Not_Exists));

				nurse.UserId = nurseInfo.UserId;
				nurse.PhoneNumber = nurseInfo.PhoneNumber;
				nurse.PlotNumber = nurseInfo.PlotNumber;
				nurse.UpdatedAt = DateTime.UtcNow;

				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return nurse;
			}
			catch (Exception ex)
			{
				await trans.RollbackAsync();
				throw new Exception(ex.Message);
			}
		});
	}

	public async Task<IPageList<INurseModel>> GetAll(ISkipTakeRq request)
	{
		return await Execute(async model =>
		{
			var total = await model.Nurses.Where(x => x.Deleted == false).CountAsync();
			var skip = Math.Min(total, request.Skip);
			var take = Math.Min(total - request.Skip, request.Take == 0 ? total : request.Take);
			return new PageList<INurseModel>
			{
				Total = total,
				First = skip,
				Items = take > 0
					? await model.Nurses
						.AsNoTracking().Where(x => x.Deleted == false)
						.Skip(skip)
						.Take(take)
						.ToListAsync<INurseModel>()
					: new List<INurseModel>()
			};
		});
	}

	public async Task<INurseModel> GetById(long nurseId)
	{
		return await Execute(async model =>
			await model.Nurses.FirstOrDefaultAsync(x => x.Id == nurseId) ??
			throw new Exception(nameof(HfdError.HFD_Nurse_Not_Found)));
	}

	public async Task<INurseModel> Delete(long nurseId)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var nurse = await model.Nurses.SingleOrDefaultAsync(x => x.Id == nurseId);
				if (nurse == null)
					throw new Exception(nameof(HfdError.HFD_Nurse_Not_Exists));
				nurse.Deleted = true;
				nurse.UpdatedAt = DateTime.UtcNow;
				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return nurse;
			}
			catch (Exception ex)
			{
				await trans.RollbackAsync();
				throw new Exception(ex.Message);
			}
		});
	}
}