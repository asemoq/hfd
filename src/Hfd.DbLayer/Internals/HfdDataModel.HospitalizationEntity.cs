using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Contract.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hfd.DbLayer.Internals;

///// <summary>
///// Data model part.
///// </summary>
public partial class HfdDataModel
{
	/// <inheritdoc cref="HospitalizationEntity" />
	/// .
	public virtual DbSet<HospitalizationEntity> Hospitalizations { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("hospitalization")]
public class HospitalizationEntity : HfdEntityBase, IHospitalizationFull
{
	#region Navigation

	/// <summary>
	///     Patient.
	/// </summary>
	[ForeignKey(nameof(PatientId))]
	[InverseProperty(nameof(Internals.PatientEntity.HospitalizationEntities))]
	public PatientEntity PatientEntity { get; set; }

	#endregion

	/// <inheritdoc />
	public string Address { get; set; }

	/// <inheritdoc />
	public string DocId { get; set; }

	/// <inheritdoc />
	public DateTime Date { get; set; }

	/// <inheritdoc />
	public long PatientId { get; set; }

	#region [ Configuration ]

	/// <inheritdoc />
	public class Configuration : IEntityTypeConfiguration<HospitalizationEntity>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<HospitalizationEntity> entity)
		{
			entity
				.HasKey(x => x.Id);

			entity
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();
		}
	}

	#endregion
}