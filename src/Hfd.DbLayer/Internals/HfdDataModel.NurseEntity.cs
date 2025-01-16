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
	/// <inheritdoc cref="NurseEntity" />
	/// .
	public virtual DbSet<NurseEntity> Nurses { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("nurse")]
public class NurseEntity : HfdEntityBase, INurseFull
{
	/// <inheritdoc />
	public string PhoneNumber { get; set; }

	/// <inheritdoc />
	public long? PlotNumber { get; set; }

	/// <inheritdoc />
	public long UserId { get; set; }

	/// <inheritdoc />
	[NotMapped]
	public IUserFull User => UserEntity;

	#region [ Configuration ]

	/// <inheritdoc />
	public class Configuration : IEntityTypeConfiguration<NurseEntity>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<NurseEntity> entity)
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

	[InverseProperty(nameof(PatientEntity.NurseEntity))]
	public virtual ICollection<PatientEntity> PatientEntities { get; set; } =
		new HashSet<PatientEntity>();

	#endregion
}