using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Base.Contract.Model;
using Hfd.Contract.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hfd.DbLayer.Internals;

///// <summary>
///// Data model part.
///// </summary>
public partial class HfdDataModel
{
	/// <inheritdoc cref="PatientEntity" />
	/// .
	public virtual DbSet<PatientEntity> Patients { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("patient")]
public class PatientEntity : HfdEntityBase, IPatientFull
{
	/// <inheritdoc />
	public long UserId { get; set; }

	/// <inheritdoc />
	public long? NurseId { get; set; }

	/// <inheritdoc />
	public string Scale { get; set; }

	/// <inheritdoc />
	public string Imt { get; set; }

	/// <inheritdoc />
	public int Klas { get; set; }

	/// <inheritdoc />
	public int PlotNumber { get; set; }

	/// <inheritdoc />
	public bool Dead { get; set; }

	/// <inheritdoc />
	public string PhoneNumber { get; set; }

	/// <inheritdoc />
	public string Gender { get; set; }

	/// <inheritdoc />
	public string Birthday { get; set; }

	/// <inheritdoc />
	public string Region { get; set; }

	/// <inheritdoc />
	public string Address { get; set; }

	/// <inheritdoc />
	public string Diagnosis { get; set; }

	/// <inheritdoc />
	public string Diseases { get; set; }

	/// <inheritdoc />
	public string Height { get; set; }

	/// <inheritdoc />
	public string Weight { get; set; }

	/// <inheritdoc />
	public string FamilyFullName { get; set; }

	/// <inheritdoc />
	public string FamilyPhoneNumber { get; set; }

	/// <inheritdoc />
	public IUserFull User => UserEntity;

	/// <inheritdoc />
	public INurseFull Nurse => NurseEntity;


	#region [ Configuration ]

	/// <inheritdoc />
	public class Configuration : IEntityTypeConfiguration<PatientEntity>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<PatientEntity> entity)
		{
			entity
				.HasKey(x => x.Id);

			entity
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			entity
				.Navigation(x => x.UserEntity)
				.AutoInclude();
		}
	}

	#endregion


	#region Navigation

	/// <summary>
	///     User.
	/// </summary>
	public UserEntity UserEntity { get; set; }

	/// <summary>
	///     Nurse.
	/// </summary>
	[ForeignKey(nameof(NurseId))]
	[InverseProperty(nameof(Internals.NurseEntity.PatientEntities))]
	public NurseEntity NurseEntity { get; set; }


	[InverseProperty(nameof(SurveyEntity.PatientEntity))]
	public virtual ICollection<SurveyEntity> SurveyEntities { get; set; } =
		new HashSet<SurveyEntity>();

	[InverseProperty(nameof(TshhEntity.PatientEntity))]
	public virtual ICollection<TshhEntity> TshhEntities { get; set; } =
		new HashSet<TshhEntity>();

	[InverseProperty(nameof(HospitalizationEntity.PatientEntity))]
	public virtual ICollection<HospitalizationEntity> HospitalizationEntities { get; set; } =
		new HashSet<HospitalizationEntity>();

	#endregion
}