using Hfd.DbLayer.Internals;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hfd.DbLayer.Configurations;

public class UserEntityConfiguration : BaseEntityConfiguration<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        base.Configure(builder);

        builder.HasOne(u => u.PatientEntity).WithOne(p => p.UserEntity).HasForeignKey<PatientEntity>(e => e.UserId).IsRequired();

        builder.HasOne(u => u.NurseEntity).WithOne(n => n.UserEntity).HasForeignKey<NurseEntity>(e => e.UserId).IsRequired();

        builder.HasOne(u => u.PsychologistEntity)
            .WithOne(p => p.UserEntity)
            .HasForeignKey<PsychologistEntity>(e => e.UserId)
            .IsRequired();

        builder.HasOne(u => u.SocialWorkerEntity)
            .WithOne(s => s.UserEntity)
            .HasForeignKey<SocialWorkerEntity>(e => e.UserId)
            .IsRequired();
    }
}
