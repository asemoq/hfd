namespace Hfd.Base.Contract.Model;

public interface IAuditLogRq : ISkipTakeRq
{
	DateTime? StartedFrom { get; }

	DateTime? StartedTo { get; }
}

public class AuditLogRq : SkipTakeRq, IAuditLogRq
{
	public DateTime? StartedFrom { get; set; }

	public DateTime? StartedTo { get; set; }
}