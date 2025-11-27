using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Common.DbLayer.Internals;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
namespace Hfd.DbLayer.Internals;

///// <summary>
///// Data model part.
///// </summary>
public partial class HfdDataModel
{
    /// <inheritdoc cref="PatientEntity" />
    /// .
    public virtual DbSet<PatientEntity> Patients { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("patient")]
public class PatientEntity : EntityBase, IPatientFull
{
    /// <inheritdoc />
    [MaxLength(36)]
    [Column("user_id")]
    public string UserId { get; set; }

    /// <inheritdoc />
    [Column("scale")]
    public string? Scale { get; set; }

    /// <inheritdoc />
    [Column("height")]
    public double Height { get; set; }

    /// <inheritdoc />
    [Column("weight")]
    public double Weight { get; set; }

    /// <inheritdoc />
    [NotMapped]
    public double Imt => this.Weight / Math.Pow(this.Height / 10, 2);

    /// <inheritdoc />
    [Column("klas")]
    public int Klas { get; set; }

    /// <inheritdoc />
    [Column("plot_id")]
    public long? PlotId { get; set; }

    /// <inheritdoc />
    [Column("dead")]
    public bool Dead { get; set; }

    /// <inheritdoc />
    [Column("gender")]
    public Gender Gender { get; set; }

    /// <inheritdoc />
    [Column("birthday")]
    public DateTime Birthday { get; set; }

    /// <inheritdoc />
    [Column("region")]
    public string? Region { get; set; }

    /// <inheritdoc />
    [Column("address")]
    public string? Address { get; set; }

    /// <inheritdoc />
    [Column("diagnosis")]
    public string? Diagnosis { get; set; }

    /// <inheritdoc />
    [Column("diseases")]
    public string? Diseases { get; set; }

    /// <inheritdoc />
    [Column("family_full_name")]
    public string FamilyFullName { get; set; }

    /// <inheritdoc />
    [Column("family_phone_number")]
    public string FamilyPhoneNumber { get; set; }

    #region Navigation

    [ForeignKey(nameof(PlotId))]
    [InverseProperty(nameof(Internals.PlotEntity.PatientEntities))]
    public PlotEntity? PlotEntity { get; set; }

    [InverseProperty(nameof(SurveyEntity.PatientEntity))]
    public virtual ICollection<SurveyEntity> SurveyEntities { get; init; } = new HashSet<SurveyEntity>();

    [InverseProperty(nameof(TshhEntity.PatientEntity))]
    public virtual ICollection<TshhEntity> TshhEntities { get; init; } = new HashSet<TshhEntity>();

    [InverseProperty(nameof(HospitalizationEntity.PatientEntity))]
    public virtual ICollection<HospitalizationEntity> HospitalizationEntities { get; init; } = new HashSet<HospitalizationEntity>();

    #endregion
}
