using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Common.DbLayer.Internals;
using Hfd.Contract.Model;

namespace Hfd.DbLayer.Internals;

///// <summary>
///// Data model part.
///// </summary>
public partial class HfdDataModel
{
    /// <inheritdoc cref="TshhEntity" />
    /// .
    public virtual DbSet<TshhEntity> Tshhs { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("tshh")]
public class TshhEntity : EntityBase, ITshhFull
{
    #region Navigation

    [ForeignKey(nameof(PatientId))]
    [InverseProperty(nameof(Internals.PatientEntity.TshhEntities))]
    public PatientEntity? PatientEntity { get; set; }

    #endregion

    /// <inheritdoc />
    [Column("klass")]
    public long Klass { get; set; }

    /// <inheritdoc />
    [Column("distance")]
    public long Distance { get; set; }

    /// <inheritdoc />
    [Column("patient_id")]
    public long PatientId { get; set; }

    /// <inheritdoc />
    [Column("creation_date")]
    public DateTime CreationDate { get; set; }
}
