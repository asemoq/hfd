using Hfd.Base.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Пациент.
/// </summary>
public interface IPatientFull : IPatientModel
{
	/// <summary>
	///     Связанный пользователь.
	/// </summary>
	IUserFull User { get; }

	/// <summary>
	///     Прикрепленная медсестра.
	/// </summary>
	INurseFull Nurse { get; }

	/// <summary>
	///     Ссылка на карточку пациента.
	/// </summary>
	long PatientCardId { get; }

	/// <summary>
	///     Ссылка на медсестру.
	/// </summary>
	long? NurseId { get; }
}

public interface IPatientModel : IBaseInfo, IPatientInfo
{
	/// <summary>
	///     Карточка пациента.
	/// </summary>
	IPatientCardModel PatientCard { get; }
}

public interface IPatientInfo
{
	/// <summary>
	///     Ссылка на пользователя.
	/// </summary>
	long UserId { get; }

	/// <summary>
	///     Шкала.
	/// </summary>
	string Scale { get; }

	string Imt { get; }

	int Klas { get; }

	int PlotNumber { get; }

	bool Dead { get; }
}