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
	/// <inheritdoc cref="PatientCardEntity" />
	/// .
	public virtual DbSet<PatientCardEntity> PatientCards { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("patient_card")]
public class PatientCardEntity : HfdEntityBase, IPatientCardFull
{
	/// <inheritdoc />
	public long? PatientId { get; set; }

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

	#region [ Configuration ]

	/// <inheritdoc />
	public class Configuration : IEntityTypeConfiguration<PatientCardEntity>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<PatientCardEntity> entity)
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