using Hfd.Common.DbLayer.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hfd.DbLayer.Configurations;

public class PlotEntityConfiguration : BaseEntityConfiguration<PlotEntity>
{
    public override void Configure(EntityTypeBuilder<PlotEntity> builder)
    {
        base.Configure(builder);
        builder.HasIndex(x => x.Number).IsUnique();
    }
}
