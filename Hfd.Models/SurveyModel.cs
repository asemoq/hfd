using System;
using Hfd.Common.Models;

namespace Hfd.Models;

public class SurveyModel : BaseModel
{
    public string PatientName { get; set; }

    public string Dyspnea { get; set; }

    public double AdValue { get; set; }

    public string Ad { get; set; }

    public double WeightValue { get; set; }

    public string Weight { get; set; }

    public string Edema { get; set; }

    public double HeartRateValue { get; set; }

    public string HeartRate { get; set; }

    public string PositionInBed { get; set; }

    public long PatientId { get; set; }

    public string DayTime { get; set; }

    public DateTime DateTime { get; set; }

    public int Score { get; set; }

    public string? ScoreZone { get; set; }
}
