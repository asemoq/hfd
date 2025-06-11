using Hfd.Common.DbLayer.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hfd.DbLayer.Configurations;

public class NurseEntityConfiguration : BaseEntityConfiguration<NurseEntity>
{
    public override void Configure(EntityTypeBuilder<NurseEntity> builder)
    {
        base.Configure(builder);
        builder.Navigation(x => x.UserEntity).AutoInclude();
    }
}
