using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hfd.DbLayer.Internals;

///// <summary>
///// Data model part.
///// </summary>
public partial class HfdDataModel
{
	/// <inheritdoc cref="TrainingEntity" />
	/// .
	public virtual DbSet<TrainingEntity> Trainings { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("training")]
public class TrainingEntity : HfdEntityBase, ITrainingFull
{
	/// <inheritdoc />
	public string Name { get; set; }

	/// <inheritdoc />
	public string Text { get; set; }

	/// <inheritdoc />
	public string FileId { get; set; }

	/// <inheritdoc />
	public FileType FileType { get; set; }

	#region [ Configuration ]

	/// <inheritdoc />
	public class Configuration : IEntityTypeConfiguration<TrainingEntity>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<TrainingEntity> entity)
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