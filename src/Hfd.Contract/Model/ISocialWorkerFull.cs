using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Опрос.
/// </summary>
public interface ISocialWorkerFull : IBaseInfo, ISocialWorkerInfo
{
}

public interface ISocialWorkerInfo
{
	/// <summary>
	///     Контактный номер.
	/// </summary>
	string PhoneNumber { get; }

	/// <summary>
	///     ФИО.
	/// </summary>
	string FullName { get; }

	/// <summary>
	///     Номер участка
	/// </summary>
	long? PlotNumber { get; }

	/// <summary>
	///     Ссылка на пользователя
	/// </summary>
	long UserId { get; }
}