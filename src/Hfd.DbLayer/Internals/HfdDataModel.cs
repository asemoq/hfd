using Hfd.Base.Contract.Services;
using Hfd.Base.DbLayer.Internals;
using Microsoft.EntityFrameworkCore;

namespace Hfd.DbLayer.Internals;

public partial class HfdDataModel : BizSpaceDataModel<HfdDataModel>
{
	protected HfdDataModel(DbContextOptions options, IUserAccessor userAccessor)
		: base(options, userAccessor)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(typeof(HfdDataModel).Assembly);
	}
}