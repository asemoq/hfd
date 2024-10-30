using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Base.Contract.Services;
using Hfd.Base.DbLayer.Services;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Hfd.Base.DbLayer.Internals;

public abstract class AuditableDataModel : DbContext, IDataModel
{
	static AuditableDataModel()
	{
		AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) =>
		{
			var auditCtx = (AuditableDataModel)context;
			var auditTrx = (AuditTrx)audit;
			if (auditTrx.Entries.Count > 0)
			{
				if (auditTrx.Trx.ScopeId == default)
				{
					auditTrx.Trx.ScopeId = auditTrx.CreatedBy;
					auditCtx.Audit.Add(auditTrx.Trx);
				}

				auditCtx.AuditEntry.AddRange(audit.Entries);
				auditTrx.Trx.StoppedAt = DateTime.UtcNow;
			}

			;
		};

		AuditManager.DefaultConfiguration.AuditEntryFactory = args
			=> new AuditEntry { CreatedDate = DateTime.UtcNow };
	}

	protected AuditableDataModel(DbContextOptions options, IUserAccessor userAccessor)
		: base(options)
	{
		UserAccessor = userAccessor;
	}

	public IUserAccessor UserAccessor { get; }

	public virtual DbSet<AuditEntity> Audit { get; set; }

	public virtual DbSet<AuditEntry> AuditEntry { get; set; }

	public virtual DbSet<AuditEntryProperty> AuditEntryProperty { get; set; }

	/// <inheritdoc />
	public override int SaveChanges()
	{
		var (audit, count) = CreateAudit();
		audit.Configuration.AutoSavePreAction(this, audit);
		base.SaveChanges();
		return count;
	}

	/// <inheritdoc />
	public override async Task<int> SaveChangesAsync(CancellationToken token = default)
	{
		if (token == default)
			token = UserAccessor.Aborted;

		var (audit, count) = await CreateAuditAsync(token);
		audit.Configuration.AutoSavePreAction(this, audit);
		await base.SaveChangesAsync(token);
		return count;
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<AuditEntity>()
			.HasMany(x => x.AuditEntry)
			.WithOne()
			.HasForeignKey(x => x.CreatedBy);
	}

	private (Audit audit, int count) CreateAudit()
	{
		var userUid = UserAccessor.UserFull == null ? string.Empty : UserAccessor.UserFull.Username;
		var trx =
			Audit.FirstOrDefault(x => x.ScopeId == UserAccessor.ScopeId)
			?? new AuditEntity
			{
				UserUid = userUid,
				StartedAt = DateTime.UtcNow
			};

		var audit = new AuditTrx(trx) { CreatedBy = UserAccessor.ScopeId };
		audit.PreSaveChanges(this);

		var count = base.SaveChanges();
		audit.PostSaveChanges();

		return (audit, count);
	}

	private async Task<(Audit audit, int count)> CreateAuditAsync(CancellationToken token)
	{
		var trx =
			await Audit.FirstOrDefaultAsync(x => x.ScopeId == UserAccessor.ScopeId, token)
			?? new AuditEntity
			{
				UserUid = UserAccessor.UserFull.Username,
				StartedAt = DateTime.UtcNow
			};

		var audit = new AuditTrx(trx) { CreatedBy = UserAccessor.ScopeId };
		audit.PreSaveChanges(this);

		var count = await base.SaveChangesAsync(token);
		audit.PostSaveChanges();

		return (audit, count);
	}

	internal class AuditTrx : Audit
	{
		public AuditTrx(AuditEntity trx)
		{
			Trx = trx;
		}

		public AuditEntity Trx { get; }
	}

	#region [ Вложенный класс : AuditEntity ]

	/// <summary>
	///     Роли (группы операций)
	/// </summary>
	[Table("Audit")]
	public class AuditEntity
	{
		[Column("scopeId")]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[MaxLength(255)]
		public string ScopeId { get; set; }

		[Column("userUid")] public string UserUid { get; set; }

		[Column("startedAt")] public DateTime StartedAt { get; set; }

		[Column("stoppedAt")] public DateTime StoppedAt { get; set; }

		#region [ Navigation ]

		public ICollection<AuditEntry> AuditEntry { get; set; } = new HashSet<AuditEntry>();

		#endregion
	}

	#endregion
}