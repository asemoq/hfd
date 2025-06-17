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
    /// <inheritdoc cref="SocialWorkerEntity" />
    /// .
    public virtual DbSet<SocialWorkerEntity> SocialWorkers { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("social_worker")]
public class SocialWorkerEntity : EntityBase, ISocialWorkerFull
{
    /// <inheritdoc />
    [MaxLength(15)]
    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    [Column("full_name")]
    public string FullName { get; set; }

    /// <inheritdoc />
    [Column("plot_number")]
    public long? PlotNumber { get; set; }

    /// <inheritdoc />
    [MaxLength(36)]
    [Column("user_id")]
    public string UserId { get; set; }
}
