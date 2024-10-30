using Hfd.Base.Contract.Dto;
using Hfd.Contract.Model;

namespace Hfd.Contract.Dto;

public class PatientCardModel : BaseModel, IPatientCardModel
{
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
}