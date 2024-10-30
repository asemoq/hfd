using Hfd.Base.Contract.Services;
using Microsoft.EntityFrameworkCore;

namespace Hfd.Base.DbLayer.Internals;

public abstract class BizSpaceDataModel<T> : AuditableDataModel
	where T : BizSpaceDataModel<T>
{
	protected BizSpaceDataModel(DbContextOptions options, IUserAccessor scopeContext)
		: base(options, scopeContext)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(typeof(DbLayer).Assembly);
	}
}