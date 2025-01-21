using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Model;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Contract.Services;
using Hfd.DbLayer.Internals;
using Microsoft.EntityFrameworkCore;

namespace Hfd.DbLayer.Services;

public class PatientDbService : HfdDbService, IPatientService
{
	/// <summary>
	///     Constructor of Patient service class.
	/// </summary>
	/// <param name="storage"></param>
	/// <param name="log"></param>
	public PatientDbService(HfdDbStorage storage, ILogger<PatientDbService> log)
		: base(storage, log)
	{
	}

	public async Task<IPatientModel> Add(IPatientInfo patientInfo)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var user = await model.Users.Include(userEntity => userEntity.PatientEntity)
					.SingleOrDefaultAsync(x => x.Id == patientInfo.UserId);
				if (user == null)
					throw new HfdException(nameof(HfdError.HFD_Patient_User_Not_Found));

				if (user.PatientEntity != null)
					throw new HfdException(nameof(HfdError.HFD_Patient_User_Exist));

				var patient = new PatientEntity
				{
					UserId = user.Id,
					Scale = patientInfo.Scale,
					Imt = patientInfo.Imt,
					Klas = patientInfo.Klas,
					PlotNumber = patientInfo.PlotNumber,
					Dead = patientInfo.Dead,
					Gender = patientInfo.Gender,
					Birthday = patientInfo.Birthday,
					Region = patientInfo.Region,
					PhoneNumber = patientInfo.PhoneNumber,
					Address = patientInfo.Address,
					Diagnosis = patientInfo.Diagnosis,
					Diseases = patientInfo.Diseases,
					Height = patientInfo.Height,
					Weight = patientInfo.Weight,
					FamilyFullName = patientInfo.FamilyFullName,
					FamilyPhoneNumber = patientInfo.FamilyPhoneNumber,
					CreatedAt = DateTime.UtcNow
				};
				await model.Patients.AddAsync(patient);
				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return patient;
			}
			catch
			{
				await trans.RollbackAsync();
				throw;
			}
		});
	}

	public async Task<IPatientModel> Edit(long patientId, IPatientInfo patientInfo)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var patient = await model.Patients.SingleOrDefaultAsync(x => x.Id == patientId);
				if (patient == null)
					throw new HfdException(nameof(HfdError.HFD_Patient_Not_Exists));

				if (patient.UserId != patientInfo.UserId)
				{
					var user = await model.Users.Include(userEntity => userEntity.PatientEntity)
						.SingleOrDefaultAsync(x => x.Id == patientInfo.UserId);
					if (user == null)
						throw new HfdException(nameof(HfdError.HFD_Patient_User_Not_Found));

					if (user.PatientEntity != null)
						throw new HfdException(nameof(HfdError.HFD_Patient_User_Exist));
				}

				patient.UserId = patientInfo.UserId;
				patient.Scale = patientInfo.Scale;
				patient.Imt = patientInfo.Imt;
				patient.Klas = patientInfo.Klas;
				patient.PlotNumber = patientInfo.PlotNumber;
				patient.Dead = patientInfo.Dead;
				patient.Gender = patientInfo.Gender;
				patient.Birthday = patientInfo.Birthday;
				patient.Region = patientInfo.Region;
				patient.PhoneNumber = patientInfo.PhoneNumber;
				patient.Address = patientInfo.Address;
				patient.Diagnosis = patientInfo.Diagnosis;
				patient.Diseases = patientInfo.Diseases;
				patient.Height = patientInfo.Height;
				patient.Weight = patientInfo.Weight;
				patient.FamilyFullName = patientInfo.FamilyFullName;
				patient.FamilyPhoneNumber = patientInfo.FamilyPhoneNumber;
				patient.UpdatedAt = DateTime.UtcNow;

				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return patient;
			}
			catch
			{
				await trans.RollbackAsync();
				throw;
			}
		});
	}

	public async Task<IPageList<IPatientModel>> GetAll(ISkipTakeRq request)
	{
		return await Execute(async model =>
		{
			var total = await model.Patients.Where(x => x.Deleted == false).CountAsync();
			var skip = Math.Min(total, request.Skip);
			var take = Math.Min(total - request.Skip, request.Take == 0 ? total : request.Take);
			return new PageList<IPatientModel>
			{
				Total = total,
				First = skip,
				Items = take > 0
					? await model.Patients
						.AsNoTracking().Where(x => x.Deleted == false)
						.Skip(skip)
						.Take(take)
						.ToListAsync<IPatientModel>()
					: new List<IPatientModel>()
			};
		});
	}

	public async Task<List<IPatientFull>> GetForRequest()
	{
		return await Execute(async model =>
		{
			return await model.Patients
				.Include(x => x.UserEntity)
				.Where(x => x.Deleted == false && x.Dead == false && x.UserEntity.Blocked == false)
				.ToListAsync<IPatientFull>();
		});
	}

	public async Task<IPatientModel> GetById(long patientId)
	{
		return await Execute(async model =>
			await model.Patients.FirstOrDefaultAsync(x => x.Id == patientId) ??
			throw new HfdException(nameof(HfdError.HFD_Patient_Not_Found)));
	}

	public async Task<IPatientModel> Delete(long patientId)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var patient = await model.Patients.SingleOrDefaultAsync(x => x.Id == patientId);
				if (patient == null)
					throw new HfdException(nameof(HfdError.HFD_Patient_Not_Exists));
				patient.Deleted = true;
				patient.UpdatedAt = DateTime.UtcNow;
				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return patient;
			}
			catch
			{
				await trans.RollbackAsync();
				throw;
			}
		});
	}
}