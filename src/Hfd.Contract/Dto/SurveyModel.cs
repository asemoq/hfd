using Hfd.Base.ApiClient.Model;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;

namespace Hfd.Contract.Dto;

public class SurveyModel : BaseModel, ISurveyModel
{
	public string PatientName { get; set; }

	/// <inheritdoc />
	public DyspneaType Dyspnea { get; set; }

	/// <inheritdoc />
	public double AdValue { get; set; }

	/// <inheritdoc />
	public AdType Ad { get; set; }

	/// <inheritdoc />
	public double WeightValue { get; set; }

	/// <inheritdoc />
	public WeightType Weight { get; set; }

	/// <inheritdoc />
	public EdemaType Edema { get; set; }

	/// <inheritdoc />
	public double HeartRateValue { get; set; }

	/// <inheritdoc />
	public HeartRateType HeartRate { get; set; }

	/// <inheritdoc />
	public PositionInBedType PositionInBed { get; set; }

	/// <inheritdoc />
	public long PatientId { get; set; }

	/// <inheritdoc />
	public DateTime DateTime { get; set; }

	public int Score { get; set; }
	public ZoneType ScoreZone { get; set; }
}