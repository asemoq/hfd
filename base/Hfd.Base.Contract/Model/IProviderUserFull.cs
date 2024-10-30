namespace Hfd.Base.Contract.Model;

/// <summary>
///     Пользователь.
/// </summary>
public interface IProviderUserFull : IBaseInfo, IProviderUserInfo
{
}

public interface IProviderUserInfo
{
	/// <summary>
	///     Идентификатор пациента.
	/// </summary>
	long PatientId { get; }

	#region [ Булевы флаги, определяющие наличие подписки на определенный провайдер ]

	/// <summary>
	///     Признак, что пользователь подписан на Whatsapp.
	/// </summary>
	bool? Whatsapp { get; }

	/// <summary>
	///     Признак, что пользователь подписан на Telegram.
	/// </summary>
	bool? Telegram { get; }

	#endregion
}