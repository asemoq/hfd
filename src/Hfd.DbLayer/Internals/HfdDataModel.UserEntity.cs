using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Enums;
using Hfd.Base.Contract.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hfd.DbLayer.Internals;

///// <summary>
///// Data model part.
///// </summary>
public partial class HfdDataModel
{
	/// <inheritdoc cref="UserEntity" />
	/// .
	public virtual DbSet<UserEntity> Users { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("user")]
public class UserEntity : HfdEntityBase, IUserFull
{
	/// <inheritdoc />
	public string Iin { get; set; }

	/// <inheritdoc />
	public LoginRole Usertype { get; set; }

	/// <inheritdoc />
	public string Username { get; set; }

	/// <inheritdoc />
	public string Password { get; set; }

	/// <inheritdoc />
	public string FullName { get; set; }

	/// <inheritdoc />
	public bool Blocked { get; set; }

	/// <inheritdoc />
	public string BlockedReason { get; set; }

	/// <inheritdoc />
	public DateTime? DeletedAt { get; set; }

	/// <inheritdoc />
	public Language Language { get; set; }

	#region [ Configuration ]

	/// <inheritdoc />
	public class Configuration : IEntityTypeConfiguration<UserEntity>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<UserEntity> entity)
		{
			entity
				.HasKey(x => x.Id);

			entity
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();

			entity
				.HasOne(u => u.PatientEntity)
				.WithOne(p => p.UserEntity)
				.HasForeignKey<PatientEntity>(e => e.UserId)
				.IsRequired();

			entity
				.HasOne(u => u.NurseEntity)
				.WithOne(n => n.UserEntity)
				.HasForeignKey<NurseEntity>(e => e.UserId)
				.IsRequired();

			entity
				.HasOne(u => u.PsychologistEntity)
				.WithOne(p => p.UserEntity)
				.HasForeignKey<PsychologistEntity>(e => e.UserId)
				.IsRequired();

			entity
				.HasOne(u => u.SocialWorkerEntity)
				.WithOne(s => s.UserEntity)
				.HasForeignKey<SocialWorkerEntity>(e => e.UserId)
				.IsRequired();
		}
	}

	#endregion

	#region Navigation

	/// <summary>
	///     Patient.
	/// </summary>
	public PatientEntity PatientEntity { get; set; }

	/// <summary>
	///     Nurse.
	/// </summary>
	public NurseEntity NurseEntity { get; set; }

	/// <summary>
	///     Psychologist.
	/// </summary>
	public PsychologistEntity PsychologistEntity { get; set; }

	/// <summary>
	///     SocialWorker.
	/// </summary>
	public SocialWorkerEntity SocialWorkerEntity { get; set; }

	#endregion
}