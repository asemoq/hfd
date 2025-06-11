using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    public long MessageId { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    public required string FileId { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    public required string UniqueId { get; set; }

    /// <inheritdoc />
    public long? Size { get; set; }

    /// <inheritdoc />
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string? Data { get; set; }
}
