using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Common.DbLayer.Internals;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;

namespace Hfd.DbLayer.Internals;

///// <summary>
///// Data model part.
///// </summary>
public partial class HfdDataModel
{
    /// <inheritdoc cref="SurveyEntity" />
    /// .
    public virtual DbSet<SurveyEntity> Surveys { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("survey")]
public class SurveyEntity : EntityBase, ISurveyFull
{
    /// <inheritdoc />
    public DyspneaType Dyspnea { get; set; }

    /// <inheritdoc />
    public double AdValue { get; set; }

    public IPatientFull PatientModel => this.PatientEntity!;

    public DayTime DayTime => this.DateTime.Hour < 12 ? DayTime.Morning : DayTime.Afternoon;

    /// <inheritdoc />
    [NotMapped]
    public AdType Ad => this.AdValue switch
    {
        >= 120 => AdType.Normal,
        < 120 and > 100 => AdType.FirstHypotension,
        <= 100 => AdType.SecondHypotension,
        _ => throw new ArgumentOutOfRangeException(),
    };

    /// <inheritdoc />
    public double WeightValue { get; set; }

    /// <inheritdoc />
    [NotMapped]
    public WeightType Weight => WeightType.Normal;

    /// <inheritdoc />
    public EdemaType Edema { get; set; }

    /// <inheritdoc />
    public double HeartRateValue { get; set; }

    /// <inheritdoc />
    [NotMapped]
    public HeartRateType HeartRate => this.HeartRateValue switch
    {
        >= 90 => HeartRateType.TachycardiaOrBradycardia,
        < 90 and > 60 => HeartRateType.Normal,
        <= 60 => HeartRateType.TachycardiaOrBradycardia,
        _ => throw new ArgumentOutOfRangeException(),
    };

    /// <inheritdoc />
    public PositionInBedType PositionInBed { get; set; }

    /// <inheritdoc />
    public long PatientId { get; set; }

    /// <inheritdoc />
    public DateTime DateTime { get; set; }

    /// <inheritdoc />
    [NotMapped]
    public int Score => (int)this.Dyspnea
                        + (int)this.Ad
                        + (int)this.Weight
                        + (int)this.Edema
                        + (int)this.HeartRate
                        + (int)this.PositionInBed;

    /// <inheritdoc />
    [NotMapped]
    public ZoneType ScoreZone => this.Score switch
    {
        >= 0 and <= 2 => ZoneType.Green,
        >= 3 and <= 6 => ZoneType.Yellow,
        >= 7 and <= 16 => ZoneType.Red,
        _ => throw new ArgumentOutOfRangeException(),
    };

    #region Navigation

    [ForeignKey(nameof(PatientId))]
    [InverseProperty(nameof(Internals.PatientEntity.SurveyEntities))]
    public PatientEntity? PatientEntity { get; set; }

    #endregion
}
