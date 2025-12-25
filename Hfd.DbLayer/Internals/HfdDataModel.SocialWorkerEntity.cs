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
    /// <inheritdoc cref="SocialWorkerEntity" />
    /// .
    public virtual DbSet<SocialWorkerEntity> SocialWorkers { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("social_worker")]
public class SocialWorkerEntity : PersonEntityBase, ISocialWorkerFull
{
    /// <inheritdoc />
    [Column("plot_id")]
    public long? PlotId { get; set; }

    #region Navigation

    [ForeignKey(nameof(PlotId))]
    [InverseProperty(nameof(Internals.PlotEntity.SocialWorkerEntities))]
    public PlotEntity? PlotEntity { get; set; }

    #endregion

    #region Configuration

    public class SocialWorkerConfiguration : PersonEntityConfiguration<SocialWorkerEntity>;

    #endregion
}
