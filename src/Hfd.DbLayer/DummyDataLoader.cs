using System.Linq;
using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Internals;
using Hfd.Base.DbLayer.Services;
using Hfd.DbLayer.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace Hfd.DbLayer;

public static class DummyDataLoaderExtensions
{
	public static void FillDbByDummyData(this IServiceProvider serviceProvider)
	{
		var factory = serviceProvider.GetService<IDataModelFactory<HfdDataModel>>();
		using (var model = factory.Create())
		{
			model.Database.BeginTransaction();
			try
			{
				var existingUser =
					model.Users.SingleOrDefault(u => u.Username == nameof(LoginRole.Admin).ToLowerInvariant());
				if (existingUser != null) return;

				var admin = new UserEntity
				{
					Usertype = LoginRole.Admin,
					Username = nameof(LoginRole.Admin).ToLowerInvariant(),
					Password = nameof(LoginRole.Admin).ToLowerInvariant().Hash(),
					Blocked = false,
					CreatedAt = DateTime.UtcNow
				};
				var nurseUser = new UserEntity
				{
					Usertype = LoginRole.Nurse,
					Username = nameof(LoginRole.Nurse).ToLowerInvariant(),
					Password = nameof(LoginRole.Nurse).ToLowerInvariant().Hash(),
					Blocked = false,
					CreatedAt = DateTime.UtcNow
				};
				var patientUser = new UserEntity
				{
					Usertype = LoginRole.Patient,
					Username = nameof(LoginRole.Patient).ToLowerInvariant(),
					Password = nameof(LoginRole.Patient).ToLowerInvariant().Hash(),
					Blocked = false,
					CreatedAt = DateTime.UtcNow
				};
				model.Users.Add(admin);
				model.Users.Add(nurseUser);
				model.Users.Add(patientUser);
				model.SaveChanges();
				var nurse = new NurseEntity
				{
					UserId = nurseUser.Id,
					PhoneNumber = nameof(NurseEntity.PhoneNumber),
					PlotNumber = 1,
					CreatedAt = DateTime.UtcNow
				};
				model.Nurses.Add(nurse);
				model.SaveChanges();
				var patient = new PatientEntity
				{
					UserId = patientUser.Id,
					Scale = nameof(PatientEntity.Scale),
					Imt = nameof(PatientEntity.Imt),
					Klas = 0,
					PlotNumber = 1,
					Dead = false,
					CreatedAt = DateTime.UtcNow
				};
				model.Patients.Add(patient);
				model.SaveChanges();
				var patientCard = new PatientCardEntity
				{
					PatientId = patient.Id,
					Gender = nameof(PatientCardEntity.Gender),
					Birthday = nameof(PatientCardEntity.Birthday),
					Region = nameof(PatientCardEntity.Region),
					PhoneNumber = nameof(PatientCardEntity.PhoneNumber),
					Address = nameof(PatientCardEntity.Address),
					Diagnosis = nameof(PatientCardEntity.Diagnosis),
					Diseases = nameof(PatientCardEntity.Diseases),
					Height = nameof(PatientCardEntity.Height),
					Weight = nameof(PatientCardEntity.Weight),
					FamilyFullName = nameof(PatientCardEntity.FamilyFullName),
					FamilyPhoneNumber = nameof(PatientCardEntity.FamilyPhoneNumber),
					CreatedAt = DateTime.UtcNow
				};
				model.PatientCards.Add(patientCard);
				model.SaveChanges();
				model.Database.CommitTransaction();
			}
			catch (Exception ex)
			{
				serviceProvider.GetService<ILogger<HfdDataModel>>()?
					.LogDebug(ex.StackTrace);
			}
		}

		serviceProvider.GetService<ILogger<HfdDataModel>>()?
			.LogDebug("Disposed data model.");
	}
}