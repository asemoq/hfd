using System.Collections.Generic;
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
    /// <inheritdoc cref="PlotEntity" />
    /// .
    public virtual DbSet<PlotEntity> Plots { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("plot")]
public class PlotEntity : EntityBase, IPlotFull
{
    /// <inheritdoc />
    [Column("number")]
    public int Number { get; set; }

    /// <inheritdoc />
    [Column("area")]
    public string Area { get; set; }

    [NotMapped]
    public IEnumerable<INurseFull> Nurses => this.NurseEntities;

    [NotMapped]
    public IEnumerable<IPatientFull> Patients => this.PatientEntities;

    [NotMapped]
    public IEnumerable<IPsychologistFull> Psychologists => this.PsychologistEntities;

    [NotMapped]
    public IEnumerable<ISocialWorkerFull> SocialWorkers => this.SocialWorkerEntities;

    #region Navigation

    [InverseProperty(nameof(PatientEntity.PlotEntity))]
    public virtual ICollection<PatientEntity> PatientEntities { get; init; } = new HashSet<PatientEntity>();

    [InverseProperty(nameof(NurseEntity.PlotEntity))]
    public virtual ICollection<NurseEntity> NurseEntities { get; init; } = new HashSet<NurseEntity>();

    [InverseProperty(nameof(PsychologistEntity.PlotEntity))]
    public virtual ICollection<PsychologistEntity> PsychologistEntities { get; init; } = new HashSet<PsychologistEntity>();

    [InverseProperty(nameof(SocialWorkerEntity.PlotEntity))]
    public virtual ICollection<SocialWorkerEntity> SocialWorkerEntities { get; init; } = new HashSet<SocialWorkerEntity>();

    #endregion
}
