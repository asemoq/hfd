using Hfd.Base.Contract.Dto;

namespace Hfd.Contract.Dto;

public class PatientModel : BaseModel
{
	public long UserId { get; set; }
	public string Scale { get; set; }
	public string Imt { get; set; }
	public int Klas { get; set; }
	public int PlotNumber { get; set; }
	public bool Dead { get; set; }
	public PatientCardModel PatientCard { get; set; }
	public UserFull User { get; set; }
}