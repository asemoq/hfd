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
    /// <inheritdoc cref="HospitalizationEntity" />
    /// .
    public virtual DbSet<HospitalizationEntity> Hospitalizations { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("hospitalization")]
public class HospitalizationEntity : EntityBase, IHospitalizationFull
{
    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    [Column("address")]
    public required string Address { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    [Column("doc_id")]
    public required string DocId { get; set; }

    /// <inheritdoc />
    [Column("creation_date")]
    public DateTime CreationDate { get; set; }

    /// <inheritdoc />
    [Column("patient_id")]
    public long PatientId { get; set; }

    #region Navigation

    /// <summary>
    ///     Patient.
    /// </summary>
    [ForeignKey(nameof(PatientId))]
    [InverseProperty(nameof(Internals.PatientEntity.HospitalizationEntities))]
    public PatientEntity? PatientEntity { get; set; }

    #endregion

    #region Configuration

    public class HospitalizationConfiguration : BaseEntityConfiguration<HospitalizationEntity>;

    #endregion
}
