using Hfd.Base.ApiClient.Model;
using Hfd.Contract.Model;

namespace Hfd.Contract.Dto;

public class NurseModel : BaseModel, INurseModel
{
	public string PhoneNumber { get; set; }
	public long? PlotNumber { get; set; }
	public long UserId { get; set; }
	public UserFull User { get; set; }
}