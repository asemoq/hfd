using Hfd.Base.Contract.Dto;
using Hfd.Contract.Model;

namespace Hfd.Contract.Dto;

public class PatientModel : BaseModel, IPatientInfo
{
	/// <inheritdoc />
	public long UserId { get; set; }

	/// <inheritdoc />
	public string Scale { get; set; }

	/// <inheritdoc />
	public string Imt { get; set; }

	/// <inheritdoc />
	public int Klas { get; set; }

	/// <inheritdoc />
	public int PlotNumber { get; set; }

	/// <inheritdoc />
	public bool Dead { get; set; }

	/// <inheritdoc />
	public string PhoneNumber { get; set; }

	/// <inheritdoc />
	public string Gender { get; set; }

	/// <inheritdoc />
	public string Birthday { get; set; }

	/// <inheritdoc />
	public string Region { get; set; }

	/// <inheritdoc />
	public string Address { get; set; }

	/// <inheritdoc />
	public string Diagnosis { get; set; }

	/// <inheritdoc />
	public string Diseases { get; set; }

	/// <inheritdoc />
	public string Height { get; set; }

	/// <inheritdoc />
	public string Weight { get; set; }

	/// <inheritdoc />
	public string FamilyFullName { get; set; }

	/// <inheritdoc />
	public string FamilyPhoneNumber { get; set; }

	public UserFull User { get; set; }
}