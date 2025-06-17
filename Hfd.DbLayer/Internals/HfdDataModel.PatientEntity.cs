using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Common.DbLayer.Internals;
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
    [Column("full_name")]
    public string FullName { get; set; }

    /// <inheritdoc />
    public long? NurseId { get; set; }

    /// <inheritdoc />
    public string? Scale { get; set; }

    /// <inheritdoc />
    public string? Imt { get; set; }

    /// <inheritdoc />
    public int Klas { get; set; }

    /// <inheritdoc />
    [Column("plot_number")]
    public int PlotNumber { get; set; }

    /// <inheritdoc />
    public bool Dead { get; set; }

    /// <inheritdoc />
    [MaxLength(20)]
    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    /// <inheritdoc />
    public string? Gender { get; set; }

    /// <inheritdoc />
    public string? Birthday { get; set; }

    /// <inheritdoc />
    public string? Region { get; set; }

    /// <inheritdoc />
    public string? Address { get; set; }

    /// <inheritdoc />
    public string? Diagnosis { get; set; }

    /// <inheritdoc />
    public string? Diseases { get; set; }

    /// <inheritdoc />
    public string? Height { get; set; }

    /// <inheritdoc />
    public string? Weight { get; set; }

    /// <inheritdoc />
    public string? FamilyFullName { get; set; }

    /// <inheritdoc />
    public string? FamilyPhoneNumber { get; set; }

    /// <inheritdoc />
    public INurseFull Nurse => this.NurseEntity!;

    #region Navigation

    /// <summary>
    ///     Nurse.
    /// </summary>
    [ForeignKey(nameof(NurseId))]
    [InverseProperty(nameof(Internals.NurseEntity.PatientEntities))]
    public NurseEntity? NurseEntity { get; set; }

    [InverseProperty(nameof(SurveyEntity.PatientEntity))]
    public virtual ICollection<SurveyEntity> SurveyEntities { get; init; } = new HashSet<SurveyEntity>();

    [InverseProperty(nameof(TshhEntity.PatientEntity))]
    public virtual ICollection<TshhEntity> TshhEntities { get; init; } = new HashSet<TshhEntity>();

    [InverseProperty(nameof(HospitalizationEntity.PatientEntity))]
    public virtual ICollection<HospitalizationEntity> HospitalizationEntities { get; init; } =
        new HashSet<HospitalizationEntity>();

    #endregion
}
