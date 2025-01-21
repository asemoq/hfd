using Hfd.Base.Contract.Model;
using Hfd.Contract.Enums;

namespace Hfd.Contract.Model;

/// <summary>
///     Опрос.
/// </summary>
public interface ISurveyFull : ISurveyModel
{
}

public interface ISurveyModel : IBaseInfo, ISurveyInfo
{
	/// <summary>
	///     Артериальное давление.
	/// </summary>
	AdType Ad { get; }

	/// <summary>
	///     Оценка пульса
	/// </summary>
	HeartRateType HeartRate { get; }

	/// <summary>
	///     Вес
	/// </summary>
	WeightType Weight { get; }

	/// <summary>
	///     Дата опроса
	/// </summary>
	DateTime Date { get; }
}

public interface ISurveyInfo
{
	/// <summary>
	///     Одышка.
	/// </summary>
	DyspneaType Dyspnea { get; }

	/// <summary>
	///     Значение артериального давления.
	/// </summary>
	double AdValue { get; }

	/// <summary>
	///     Значение веса.
	/// </summary>
	double WeightValue { get; }

	/// <summary>
	///     Отек
	/// </summary>
	EdemaType Edema { get; }

	/// <summary>
	///     Пульс
	/// </summary>
	double HeartRateValue { get; }

	/// <summary>
	///     Позиция в кровати
	/// </summary>
	PositionInBedType PositionInBed { get; }

	/// <summary>
	///     Ссылка на пациента
	/// </summary>
	long PatientId { get; }

	// /// <summary>
	// /// Фото позиции в кровати
	// /// </summary>
	// byte[] PositionInBedPhoto { get; }
	//
	// /// <summary>
	// /// Фото отека
	// /// </summary>
	// byte[] EdemaPhoto { get; }
}