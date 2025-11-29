using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Common.DbLayer.Configurations;
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
    [Column("dyspnea")]
    public DyspneaType Dyspnea { get; set; }

    /// <inheritdoc />
    [Column("ad_value")]
    public double AdValue { get; set; }

    [NotMapped]
    public IPatientFull PatientModel => this.PatientEntity!;

    [NotMapped]
    public DayTime DayTime => this.DateTime.ToLocalTime().Hour < (int)DayTime.Afternoon ? DayTime.Morning : DayTime.Afternoon;

    /// <inheritdoc />
    [NotMapped]
    public AdType Ad => this.AdValue switch
    {
        >= 120 => AdType.Normal,
        < 120 and > 100 => AdType.FirstHypotension,
        _ => AdType.SecondHypotension,
    };

    /// <inheritdoc />
    [Column("weight_value")]
    public double WeightValue { get; set; }

    /// <inheritdoc />
    [NotMapped]
    public WeightType Weight => WeightType.Normal;

    /// <inheritdoc />
    [Column("edema")]
    public EdemaType Edema { get; set; }

    /// <inheritdoc />
    [Column("heart_rate_value")]
    public double HeartRateValue { get; set; }

    /// <inheritdoc />
    [NotMapped]
    public HeartRateType HeartRate => this.HeartRateValue switch
    {
        >= 90 => HeartRateType.TachycardiaOrBradycardia,
        < 90 and > 60 => HeartRateType.Normal,
        _ => HeartRateType.TachycardiaOrBradycardia,
    };

    /// <inheritdoc />
    [Column("position_in_bed")]
    public PositionInBedType PositionInBed { get; set; }

    /// <inheritdoc />
    [Column("patient_id")]
    public long PatientId { get; set; }

    /// <inheritdoc />
    [Column("date_time")]
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
    public ZoneType? ScoreZone => this.Score switch
    {
        >= 0 and <= 2 => ZoneType.Green,
        >= 3 and <= 6 => ZoneType.Yellow,
        >= 7 and <= 16 => ZoneType.Red,
        _ => null,
    };

    #region Navigation

    [ForeignKey(nameof(PatientId))]
    [InverseProperty(nameof(Internals.PatientEntity.SurveyEntities))]
    public PatientEntity? PatientEntity { get; set; }

    #endregion

    #region Configuration

    public class SurveyConfiguration : BaseEntityConfiguration<SurveyEntity>;

    #endregion
}
