using System.Data.Entity;
using Hfd.Base.Contract.Model;
using Hfd.Base.Contract.Services;
using Hfd.Base.DbLayer.Internals;
using Microsoft.Extensions.Logging;

namespace Hfd.Base.DbLayer.Services;

public abstract class AuditLogDbService<TDbStorage, TDataModel>
	: DbService<TDbStorage, TDataModel>
		, IAuditLogDbService
	where TDbStorage : DbStorage<TDataModel>
	where TDataModel : AuditableDataModel
{
	/// <summary>
	///     Initializes a new instance of the <see cref="AuditLogDbService{TDbStorage, TDataModel}" /> class.
	/// </summary>
	/// <param name="storage">Репозиторий приложения.</param>
	/// <param name="log">Журнал приложения.</param>
	protected AuditLogDbService(TDbStorage storage, ILogger log)
		: base(storage, log)
	{
	}

	public virtual async Task<AuditLogRs> GetAll(IAuditLogRq rq)
	{
		return await Execute(async model =>
		{
			var query = model.AuditEntry
				.Include(x => x.Properties)
				.OrderByDescending(x => x.CreatedDate)
				.ThenBy(x => x.CreatedBy);

			var count = await query.CountAsync();

			var entries = await query
				.Skip(rq.Skip)
				.Take(rq.Take == 0 ? int.MaxValue : rq.Take)
				.ToListAsync();

			var ids = entries.Select(x => x.CreatedBy).Distinct().ToList();

			var groups = await model.Audit
				.Where(x => ids.Contains(x.ScopeId))
				.ToListAsync();

			return new AuditLogRs
			{
				Groups = groups
					.Select(x => new AuditGroup
					{
						GroupId = x.ScopeId,
						UserUid = x.UserUid,
						StartedAt = x.StartedAt
					})
					.ToDictionary(x => x.GroupId),
				First = rq.Skip,
				Total = count,
				Items = entries
					.Select(x => new AuditEntry
					{
						GroupId = x.CreatedBy,
						Action = x.StateName,
						Entity = x.EntitySetName,
						Fields = x.Properties
							.Select(f => new AuditField
							{
								Field = f.PropertyName,
								ValueBefore = f.OldValueFormatted,
								ValueAfter = f.NewValueFormatted
							})
							.ToList()
					})
					.ToList()
			};
		});
	}
}