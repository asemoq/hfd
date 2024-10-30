using Hfd.Base.Contract.Model;

namespace Hfd.Base.Contract.Services;

public interface IAuditLogDbService : IDbService
{
	/// <summary>
	///     Возвращает бизнес-единицы подсистемы.
	/// </summary>
	/// <returns>Бизнес-единицы</returns>
	Task<AuditLogRs> GetAll(IAuditLogRq rq);
}