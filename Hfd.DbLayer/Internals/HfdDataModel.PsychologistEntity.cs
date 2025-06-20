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
    /// <inheritdoc cref="PsychologistEntity" />
    /// .
    public virtual DbSet<PsychologistEntity> Psychologists { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("psychologist")]
public class PsychologistEntity : EntityBase, IPsychologistFull
{
    /// <inheritdoc />
    [Column("plot_id")]
    public long? PlotId { get; set; }

    /// <inheritdoc />
    [MaxLength(36)]
    [Column("user_id")]
    public string UserId { get; set; }

    #region Navigation

    [ForeignKey(nameof(PlotId))]
    [InverseProperty(nameof(Internals.PlotEntity.PsychologistEntities))]
    public PlotEntity? PlotEntity { get; set; }

    #endregion
}
