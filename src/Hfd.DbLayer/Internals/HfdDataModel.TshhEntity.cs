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
	/// <inheritdoc cref="TshhEntity" />
	/// .
	public virtual DbSet<TshhEntity> Tshhs { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("tshh")]
public class TshhEntity : HfdEntityBase, ITshhFull
{
	#region Navigation

	/// <summary>
	///     Patient.
	/// </summary>
	[ForeignKey(nameof(PatientId))]
	[InverseProperty(nameof(Internals.PatientEntity.TshhEntities))]
	public PatientEntity PatientEntity { get; set; }

	#endregion

	/// <inheritdoc />
	public long Klass { get; set; }

	/// <inheritdoc />
	public long Distance { get; set; }

	/// <inheritdoc />
	public long PatientId { get; set; }

	/// <inheritdoc />
	public DateTime Date { get; set; }

	#region [ Configuration ]

	/// <inheritdoc />
	public class Configuration : IEntityTypeConfiguration<TshhEntity>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<TshhEntity> entity)
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