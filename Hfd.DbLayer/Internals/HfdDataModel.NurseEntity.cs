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
    /// <inheritdoc cref="NurseEntity" />
    /// .
    public virtual DbSet<NurseEntity> Nurses { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("nurse")]
public class NurseEntity : PersonEntityBase, INurseFull
{
    /// <inheritdoc />
    [Column("plot_id")]
    public long? PlotId { get; set; }

    #region Navigation

    [ForeignKey(nameof(PlotId))]
    [InverseProperty(nameof(Internals.PlotEntity.NurseEntities))]
    public PlotEntity? PlotEntity { get; set; }

    #endregion

    #region Configuration

    public class NurseConfiguration : PersonEntityConfiguration<NurseEntity>;

    #endregion
}
