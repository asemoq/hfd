using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Common.DbLayer.Configurations;
using Hfd.Common.DbLayer.Internals;
using Hfd.Contract.Model;

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
public class FileEntity : EntityBase, IFileFull
{
    /// <inheritdoc />
    [Column("message_id")]
    public long MessageId { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    [Column("file_id")]
    public required string FileId { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    [Column("unique_id")]
    public required string UniqueId { get; set; }

    /// <inheritdoc />
    [Column("size")]
    public long? Size { get; set; }

    /// <inheritdoc />
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    [Column("data")]
    public string? Data { get; set; }

    #region Configuration

    public class FileConfiguration : BaseEntityConfiguration<FileEntity>;

    #endregion
}
