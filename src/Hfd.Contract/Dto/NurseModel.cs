using Hfd.Base.Contract.Dto;
using Hfd.Contract.Model;

namespace Hfd.Contract.Dto;

public class NurseModel : BaseModel, INurseModel
{
	/// <inheritdoc />
	public string PhoneNumber { get; set; }

	/// <inheritdoc />
	public long? PlotNumber { get; set; }

	/// <inheritdoc />
	public long UserId { get; set; }
}