using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Helpers;
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

	public async Task<INurseFull> Add(INurseInfo nurseInfo)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var user = await model.Users.Include(userEntity => userEntity.NurseEntity)
					.SingleOrDefaultAsync(x => x.Id == nurseInfo.UserId);
				if (user == null)
					throw new HfdException(nameof(HfdError.HFD_Nurse_User_Not_Found));

				if (user.NurseEntity != null)
					throw new HfdException(nameof(HfdError.HFD_Nurse_User_Exist));

				var nurse = new NurseEntity
				{
					UserId = user.Id,
					PhoneNumber = FieldFormatHelper.GetFormattedPhoneNumber(nurseInfo.PhoneNumber),
					PlotNumber = nurseInfo.PlotNumber,
					CreatedAt = DateTime.UtcNow
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
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var nurse = await model.Nurses.SingleOrDefaultAsync(x => x.Id == nurseId);
				if (nurse == null)
					throw new HfdException(nameof(HfdError.HFD_Nurse_Not_Exists));

				if (nurse.UserId != nurseInfo.UserId)
				{
					var user = await model.Users.Include(userEntity => userEntity.NurseEntity)
						.SingleOrDefaultAsync(x => x.Id == nurseInfo.UserId);
					if (user == null)
						throw new HfdException(nameof(HfdError.HFD_Nurse_User_Not_Found));

					if (user.NurseEntity != null)
						throw new HfdException(nameof(HfdError.HFD_Nurse_User_Exist));
				}


				nurse.UserId = nurseInfo.UserId;
				nurse.PhoneNumber = FieldFormatHelper.GetFormattedPhoneNumber(nurseInfo.PhoneNumber);
				nurse.PlotNumber = nurseInfo.PlotNumber;
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
		return await Execute(async model =>
		{
			var total = await model.Nurses.CountAsync();
			var skip = Math.Min(total, request.Skip);
			var take = Math.Min(total - request.Skip, request.Take == 0 ? total : request.Take);
			return new PageList<INurseFull>
			{
				Total = total,
				First = skip,
				Items = take > 0
					? await model.Nurses
						.AsNoTracking()
						.Skip(skip)
						.Take(take)
						.ToListAsync<INurseFull>()
					: new List<INurseFull>()
			};
		});
	}

	public async Task<INurseFull> GetById(long nurseId)
	{
		return await Execute(async model =>
			await model.Nurses.SingleOrDefaultAsync(x => x.Id == nurseId) ??
			throw new HfdException(nameof(HfdError.HFD_Nurse_Not_Found)));
	}

	public async Task<INurseFull> GetByUserId(long userId)
	{
		return await Execute(async model =>
			await model.Nurses.SingleOrDefaultAsync(x => x.UserId == userId) ??
			throw new HfdException(nameof(HfdError.HFD_Nurse_Not_Found)));
	}

	public async Task<INurseFull> Delete(long nurseId)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var nurse = await model.Nurses.SingleOrDefaultAsync(x => x.Id == nurseId);
				if (nurse == null)
					throw new HfdException(nameof(HfdError.HFD_Nurse_Not_Exists));
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