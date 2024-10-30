namespace Hfd.Base.Contract.Model;

public class AuditLogRs : PageList<AuditEntry>
{
	public Dictionary<string, AuditGroup> Groups { get; set; }
}

public class AuditGroup
{
	/// <summary>
	///     Идентификатор пользователя.
	/// </summary>
	public string UserUid { get; set; }

	/// <summary>
	///     Идентификатор группы.
	/// </summary>
	public string GroupId { get; set; }

	/// <summary>
	///     Дата операции.
	/// </summary>
	public DateTime StartedAt { get; set; }
}

public class AuditEntry
{
	/// <summary>
	///     Идентификатор группы.
	/// </summary>
	public string GroupId { get; set; }

	/// <summary>
	///     Название действия.
	/// </summary>
	public string Action { get; set; }

	/// <summary>
	///     Название сущности (таблицы).
	/// </summary>
	public string Entity { get; set; }

	/// <summary>
	///     Свойства сущности.
	/// </summary>
	public List<AuditField> Fields { get; set; }
}

public class AuditField
{
	/// <summary>
	///     Название свойства.
	/// </summary>
	public string Field { get; set; }

	/// <summary>
	///     Значение свойства до.
	/// </summary>
	public string ValueBefore { get; set; }

	/// <summary>
	///     Значение свойства после.
	/// </summary>
	public string ValueAfter { get; set; }
}