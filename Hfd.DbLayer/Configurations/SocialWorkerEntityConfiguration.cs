using Hfd.Common.DbLayer.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hfd.DbLayer.Configurations;

public class SocialWorkerEntityConfiguration : BaseEntityConfiguration<SocialWorkerEntity>
{
    public override void Configure(EntityTypeBuilder<SocialWorkerEntity> builder)
    {
        base.Configure(builder);
        builder.Navigation(x => x.UserEntity).AutoInclude();
    }
}
