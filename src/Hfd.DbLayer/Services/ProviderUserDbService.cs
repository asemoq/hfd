using System.Threading.Tasks;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Model;
using Hfd.Base.Contract.Services;
using Hfd.Contract.Enums;
using Hfd.DbLayer.Internals;
using Microsoft.EntityFrameworkCore;

namespace Hfd.DbLayer.Services;

public class ProviderUserDbService : HfdDbService, IProviderUserService
{
	/// <summary>
	///     Constructor of ProviderUser service class.
	/// </summary>
	/// <param name="storage"></param>
	/// <param name="log"></param>
	public ProviderUserDbService(HfdDbStorage storage, ILogger<ProviderUserDbService> log)
		: base(storage, log)
	{
	}

	/// <inheritdoc />
	public async Task<IProviderUserFull> Add(long patientId, bool? whatsapp, bool? telegram)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var user = await model.ProviderUsers.SingleOrDefaultAsync(x => x.PatientId == patientId);
				if (user != null) return user;
				user = new ProviderUserEntity
				{
					PatientId = patientId,
					Whatsapp = whatsapp,
					Telegram = telegram,
					CreatedAt = DateTime.UtcNow
				};
				await model.ProviderUsers.AddAsync(user);
				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return user;
			}
			catch
			{
				await trans.RollbackAsync();
				throw;
			}
		});
	}

	/// <inheritdoc />
	public async Task<IProviderUserFull> Edit(long userId, long patientId, bool? whatsapp, bool? telegram)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var user = await model.ProviderUsers.SingleOrDefaultAsync(x => x.Id == userId);
				if (user == null)
					throw new HfdException(nameof(HfdError.HFD_ProviderUser_Not_Exists));

				user.PatientId = patientId;
				user.Whatsapp = whatsapp;
				user.Telegram = telegram;
				user.UpdatedAt = DateTime.UtcNow;
				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return user;
			}
			catch
			{
				await trans.RollbackAsync();
				throw;
			}
		});
	}

	public async Task<IProviderUserFull> GetByPatientId(long patientId)
	{
		return await Execute(async model =>
			await model.ProviderUsers.FirstOrDefaultAsync(x => x.PatientId == patientId) ??
			throw new HfdException(nameof(HfdError.HFD_ProviderUser_Not_Found)));
	}

	public async Task<IProviderUserFull> SetTelegramSubscribedStatus(long userId, bool subscribed)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var user = await model.ProviderUsers.SingleOrDefaultAsync(x => x.Id == userId);
				if (user == null)
					throw new HfdException(nameof(HfdError.HFD_ProviderUser_Not_Exists));
				user.Telegram = subscribed;
				user.UpdatedAt = DateTime.UtcNow;
				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return user;
			}
			catch
			{
				await trans.RollbackAsync();
				throw;
			}
		});
	}

	/// <inheritdoc />
	public async Task<int> Delete(long userId)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var user = await model.ProviderUsers.SingleOrDefaultAsync(x => x.Id == userId);
				if (user == null)
					throw new HfdException(nameof(HfdError.HFD_ProviderUser_Not_Exists));
				model.Remove(user);
				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return 1;
			}
			catch
			{
				await trans.RollbackAsync();
				throw;
			}
		});
	}
}