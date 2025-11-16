using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Models;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Contract.Services;

namespace Hfd.DbLayer.Services;

public class PatientDbService(HfdDbStorage storage, ILogger<PatientDbService> log)
    : HfdDbService(storage, log), IPatientService
{
    public async Task<IPatientFull> Add(IPatientInfo patientInfo)
    {
        return await this.Execute(async model =>
        {
            await using var trans = await model.Database.BeginTransactionAsync();
            try
            {
                var patient = new PatientEntity
                {
                    UserId = patientInfo.UserId,
                    PlotId = patientInfo.PlotId,
                    Scale = patientInfo.Scale,
                    Imt = patientInfo.Imt,
                    Klas = patientInfo.Klas,
                    Dead = patientInfo.Dead,
                    Gender = patientInfo.Gender,
                    Birthday = patientInfo.Birthday,
                    Region = patientInfo.Region,
                    Address = patientInfo.Address,
                    Diagnosis = patientInfo.Diagnosis,
                    Diseases = patientInfo.Diseases,
                    Height = patientInfo.Height,
                    Weight = patientInfo.Weight,
                    FamilyFullName = patientInfo.FamilyFullName,
                    FamilyPhoneNumber = patientInfo.FamilyPhoneNumber,
                    CreatedAt = DateTime.UtcNow,
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

    public async Task<IPatientFull> Edit(long patientId, IPatientInfo patientInfo)
    {
        return await this.Execute(async model =>
        {
            await using var trans = await model.Database.BeginTransactionAsync();
            try
            {
                var patient = await model.Patients.SingleOrDefaultAsync(x => x.Id == patientId);
                if (patient == null)
                {
                    throw new HfdException(nameof(HfdError.HFDPatientNotExists));
                }

                patient.UserId = patientInfo.UserId;
                patient.PlotId = patientInfo.PlotId;
                patient.Scale = patientInfo.Scale;
                patient.Imt = patientInfo.Imt;
                patient.Klas = patientInfo.Klas;
                patient.Dead = patientInfo.Dead;
                patient.Gender = patientInfo.Gender;
                patient.Birthday = patientInfo.Birthday;
                patient.Region = patientInfo.Region;
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

    public async Task<IPageList<IPatientFull>> GetAll(ISkipTakeRq request, long? nurseId)
    {
        return await this.Execute(async model =>
        {
            Expression<Func<PatientEntity, bool>> query = x => true;

            // if (nurseId != null)
            // {
            //     query = query.AndExpression(x => x.NurseId == nurseId);
            // }

            var total = await model.Patients.Where(query).CountAsync();
            var skip = Math.Min(total, request.Skip);
            var take = Math.Min(total - request.Skip, request.Take == 0 ? total : request.Take);
            return new PageList<IPatientFull>
            {
                Total = total,
                First = skip,
                Items = take > 0
                    ? await model.Patients.AsNoTracking()
                        .Where(query)
                        .Skip(skip)
                        .Take(take)
                        .OrderBy(x => x.Id)
                        .ToListAsync<IPatientFull>()
                    : new List<IPatientFull>(),
            };
        });
    }

    public async Task<List<IPatientFull>> GetForRequest()
    {
        return await this.Execute(async model =>
        {
            return await model.Patients.Where(x => x.Dead == false).ToListAsync<IPatientFull>();
        });
    }

    public async Task<IPatientFull> GetById(long patientId)
    {
        return await this.Execute(async model =>
        {
            return await model.Patients.SingleOrDefaultAsync(x => x.Id == patientId)
                   ?? throw new HfdException(nameof(HfdError.HFDPatientNotFound));
        });
    }

    public async Task<IPatientFull> GetByUserId(string userId)
    {
        return await this.Execute(async model => await model.Patients.SingleOrDefaultAsync(x => x.UserId == userId)
                                                 ?? throw new HfdException(nameof(HfdError.HFDPatientNotFound)));
    }

    public async Task<IPatientFull> Delete(long patientId)
    {
        return await this.Execute(async model =>
        {
            await using var trans = await model.Database.BeginTransactionAsync();
            try
            {
                var patient = await model.Patients.SingleOrDefaultAsync(x => x.Id == patientId);
                if (patient == null)
                {
                    throw new HfdException(nameof(HfdError.HFDPatientNotExists));
                }

                model.Remove(patient);
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
