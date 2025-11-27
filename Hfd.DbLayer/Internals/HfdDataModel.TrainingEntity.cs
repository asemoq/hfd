using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Common.DbLayer.Configurations;
using Hfd.Common.DbLayer.Internals;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;

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
public class TrainingEntity : EntityBase, ITrainingFull
{
    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    [Column("name")]
    public required string Name { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    [Column("text")]
    public required string Text { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    [Column("file_id")]
    public required string FileId { get; set; }

    /// <inheritdoc />
    [Column("file_type")]
    public FileType FileType { get; set; }

    #region Configuration

    public class TrainingConfiguration : BaseEntityConfiguration<TrainingEntity>;

    #endregion
}
