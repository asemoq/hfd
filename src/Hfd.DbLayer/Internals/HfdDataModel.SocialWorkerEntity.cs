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
	/// <inheritdoc cref="SocialWorkerEntity" />
	/// .
	public virtual DbSet<SocialWorkerEntity> SocialWorkers { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("social_worker")]
public class SocialWorkerEntity : HfdEntityBase, ISocialWorkerFull
{
	#region Navigation

	/// <summary>
	///     User.
	/// </summary>
	public UserEntity UserEntity { get; set; }

	#endregion

	/// <inheritdoc />
	public string PhoneNumber { get; set; }

	/// <inheritdoc />
	public string FullName { get; set; }

	/// <inheritdoc />
	public long? PlotNumber { get; set; }

	/// <inheritdoc />
	public long UserId { get; set; }

	#region [ Configuration ]

	/// <inheritdoc />
	public class Configuration : IEntityTypeConfiguration<SocialWorkerEntity>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<SocialWorkerEntity> entity)
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