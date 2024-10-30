using System.ComponentModel.DataAnnotations.Schema;
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
	/// <inheritdoc cref="ProviderUserEntity" />
	/// .
	public virtual DbSet<ProviderUserEntity> ProviderUsers { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("provider_user")]
public class ProviderUserEntity : HfdEntityBase, IProviderUserFull
{
	public SubscribedProviderFlag SubscribedFlags { get; set; }
	public long PatientId { get; set; }

	/// <inheritdoc />
	[NotMapped]
	public bool? Whatsapp
	{
		get => SubscribedFlags.HasFlag(SubscribedProviderFlag.Whatsapp) ? true : null;
		set => SubscribedFlags = SubscribedFlags.SetFlag(SubscribedProviderFlag.Whatsapp, value == true);
	}

	/// <inheritdoc />
	[NotMapped]
	public bool? Telegram
	{
		get => SubscribedFlags.HasFlag(SubscribedProviderFlag.Telegram) ? true : null;
		set => SubscribedFlags = SubscribedFlags.SetFlag(SubscribedProviderFlag.Telegram, value == true);
	}

	#region [ Configuration ]

	/// <inheritdoc />
	public class Configuration : IEntityTypeConfiguration<ProviderUserEntity>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<ProviderUserEntity> entity)
		{
			entity
				.HasKey(x => x.Id);

			entity
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();
		}
	}

	#endregion
}