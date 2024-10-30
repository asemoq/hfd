using Hfd.Base.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Опрос.
/// </summary>
public interface IHospitalizationFull : IBaseInfo, IHospitalizationInfo
{
}

public interface IHospitalizationInfo
{
	/// <summary>
	///     Адрес.
	/// </summary>
	string Address { get; }

	/// <summary>
	/// </summary>
	string DocId { get; }

	/// <summary>
	/// </summary>
	DateTime Date { get; }

	/// <summary>
	///     Ссылка на пациента
	/// </summary>
	long PatientId { get; }
}