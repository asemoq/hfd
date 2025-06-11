using Hfd.Common.DbLayer.Configurations;
using Hfd.DbLayer.Internals;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hfd.DbLayer.Configurations;

public class PsychologistEntityConfiguration : BaseEntityConfiguration<PsychologistEntity>
{
    public override void Configure(EntityTypeBuilder<PsychologistEntity> builder)
    {
        base.Configure(builder);
        builder.Navigation(x => x.UserEntity).AutoInclude();
    }
}
