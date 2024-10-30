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
	public long PatientCardId { get; set; }

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
	public IPatientCardModel PatientCard => PatientCardEntity;

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
				.HasOne(e => e.PatientCardEntity)
				.WithOne()
				.HasForeignKey<PatientCardEntity>(p => p.PatientId)
				.IsRequired();

			entity
				.Navigation(x => x.PatientCardEntity)
				.AutoInclude();
		}
	}

	#endregion


	#region Navigation

	/// <summary>
	///     Patient card.
	/// </summary>
	public PatientCardEntity PatientCardEntity { get; set; }

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