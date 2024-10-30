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
	/// <inheritdoc cref="FileEntity" />
	/// .
	public virtual DbSet<FileEntity> Files { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("file")]
public class FileEntity : HfdEntityBase, IFileFull
{
	/// <inheritdoc />
	public long MessageId { get; set; }

	/// <inheritdoc />
	public string FileId { get; set; }

	/// <inheritdoc />
	public string UniqueId { get; set; }

	/// <inheritdoc />
	public long? Size { get; set; }

	/// <inheritdoc />
	public string Object { get; set; }

	// #region Navigation
	//
	// /// <summary>
	// ///     Message.
	// /// </summary>
	// [ForeignKey(nameof(MessageId))]
	// [InverseProperty("FileEntities")]
	// public MessageEntity MessageEntity { get; set; }
	//
	// #endregion

	#region [ Configuration ]

	/// <inheritdoc />
	public class Configuration : IEntityTypeConfiguration<FileEntity>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<FileEntity> entity)
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