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

	public async Task<IPatientModel> Add(IPatientInfo patientInfo, IPatientCardInfo patientCardInfo)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var patient = new PatientEntity
				{
					UserId = patientInfo.UserId,
					Scale = patientInfo.Scale,
					Imt = patientInfo.Imt,
					Klas = patientInfo.Klas,
					PlotNumber = patientInfo.PlotNumber,
					Dead = patientInfo.Dead,
					CreatedAt = DateTime.UtcNow
				};
				await model.Patients.AddAsync(patient);
				await model.SaveChangesAsync();

				var patientCard = new PatientCardEntity
				{
					PatientId = patient.Id,
					Gender = patientCardInfo.Gender,
					Birthday = patientCardInfo.Birthday,
					Region = patientCardInfo.Region,
					PhoneNumber = patientCardInfo.PhoneNumber,
					Address = patientCardInfo.Address,
					Diagnosis = patientCardInfo.Diagnosis,
					Diseases = patientCardInfo.Diseases,
					Height = patientCardInfo.Height,
					Weight = patientCardInfo.Weight,
					FamilyFullName = patientCardInfo.FamilyFullName,
					FamilyPhoneNumber = patientCardInfo.FamilyPhoneNumber,
					CreatedAt = DateTime.UtcNow
				};

				await model.PatientCards.AddAsync(patientCard);
				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return patient;
			}
			catch (Exception ex)
			{
				await trans.RollbackAsync();
				throw new Exception(ex.Message);
			}
		});
	}

	public async Task<IPatientModel> Edit(long patientId, IPatientInfo patientInfo, IPatientCardInfo patientCardInfo)
	{
		return await Execute(async model =>
		{
			await using var trans = await model.Database.BeginTransactionAsync();
			try
			{
				var patient = await model.Patients.SingleOrDefaultAsync(x => x.Id == patientId);
				if (patient == null)
					throw new Exception(nameof(HfdError.HFD_Patient_Not_Exists));

				patient.UserId = patientInfo.UserId;
				patient.Scale = patientInfo.Scale;
				patient.Imt = patientInfo.Imt;
				patient.Klas = patientInfo.Klas;
				patient.PlotNumber = patientInfo.PlotNumber;
				patient.Dead = patientInfo.Dead;
				patient.UpdatedAt = DateTime.UtcNow;

				var patientCard = await model.PatientCards.SingleOrDefaultAsync(x => x.Id == patient.PatientCardId);
				if (patientCard == null)
					throw new Exception(nameof(HfdError.HFD_PatientCard_Not_Exists));

				patientCard.Gender = patientCardInfo.Gender;
				patientCard.Birthday = patientCardInfo.Birthday;
				patientCard.Region = patientCardInfo.Region;
				patientCard.PhoneNumber = patientCardInfo.PhoneNumber;
				patientCard.Address = patientCardInfo.Address;
				patientCard.Diagnosis = patientCardInfo.Diagnosis;
				patientCard.Diseases = patientCardInfo.Diseases;
				patientCard.Height = patientCardInfo.Height;
				patientCard.Weight = patientCardInfo.Weight;
				patientCard.FamilyFullName = patientCardInfo.FamilyFullName;
				patientCard.FamilyPhoneNumber = patientCardInfo.FamilyPhoneNumber;
				patientCard.UpdatedAt = DateTime.UtcNow;

				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return patient;
			}
			catch (Exception ex)
			{
				await trans.RollbackAsync();
				throw new Exception(ex.Message);
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
			throw new Exception(nameof(HfdError.HFD_Patient_Not_Found)));
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
					throw new Exception(nameof(HfdError.HFD_Patient_Not_Exists));
				patient.Deleted = true;
				patient.UpdatedAt = DateTime.UtcNow;
				await model.SaveChangesAsync();
				await trans.CommitAsync();
				return patient;
			}
			catch (Exception ex)
			{
				await trans.RollbackAsync();
				throw new Exception(ex.Message);
			}
		});
	}
}