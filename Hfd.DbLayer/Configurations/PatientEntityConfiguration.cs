using Hfd.Common.DbLayer.Configurations;
using Hfd.DbLayer.Internals;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hfd.DbLayer.Configurations;

public class PatientEntityConfiguration : BaseEntityConfiguration<PatientEntity>
{
    public override void Configure(EntityTypeBuilder<PatientEntity> builder)
    {
        base.Configure(builder);
        builder.Navigation(x => x.UserEntity).AutoInclude();
    }
}
