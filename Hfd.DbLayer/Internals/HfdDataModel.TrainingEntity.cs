using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Common.DbLayer.Internals;
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
public class TrainingEntity : EntityBase, ITrainingFull
{
    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    public required string Name { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    public required string Text { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    public required string FileId { get; set; }

    /// <inheritdoc />
    public FileType FileType { get; set; }
}
