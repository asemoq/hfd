using Hfd.Base.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Пациент.
/// </summary>
public interface IPatientCardFull : IPatientCardModel
{
	/// <summary>
	///     Ссылка на пациента.
	/// </summary>
	long? PatientId { get; }
}

public interface IPatientCardModel : IBaseInfo, IPatientCardInfo
{
}

public interface IPatientCardInfo
{
	/// <summary>
	///     Контактный номер.
	/// </summary>
	string PhoneNumber { get; }

	/// <summary>
	///     Пол.
	/// </summary>
	string Gender { get; }

	/// <summary>
	///     Дата рождения.
	/// </summary>
	string Birthday { get; }

	/// <summary>
	///     Регион.
	/// </summary>
	string Region { get; }

	/// <summary>
	///     Адрес.
	/// </summary>
	string Address { get; }

	/// <summary>
	///     Диагноз.
	/// </summary>
	string Diagnosis { get; }

	/// <summary>
	///     Болезни.
	/// </summary>
	string Diseases { get; }

	/// <summary>
	///     Рост.
	/// </summary>
	string Height { get; }

	/// <summary>
	///     Вес.
	/// </summary>
	string Weight { get; }

	/// <summary>
	///     ФИО родственника.
	/// </summary>
	string FamilyFullName { get; }

	/// <summary>
	///     Контактный номер родственника.
	/// </summary>
	string FamilyPhoneNumber { get; }
}