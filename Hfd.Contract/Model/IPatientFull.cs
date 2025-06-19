using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Пациент.
/// </summary>
public interface IPatientFull : IPersonInfo, IPatientInfo
{
    /// <summary>
    ///     Прикрепленная медсестра.
    /// </summary>
    INurseFull Nurse { get; }

    /// <summary>
    ///     Ссылка на медсестру.
    /// </summary>
    long? NurseId { get; }
}

public interface IPatientInfo
{
    /// <summary>
    ///     Шкала.
    /// </summary>
    string? Scale { get; }

    string? Imt { get; }

    int Klas { get; }

    int PlotNumber { get; }

    bool Dead { get; }

    /// <summary>
    ///     Пол.
    /// </summary>
    string? Gender { get; }

    /// <summary>
    ///     Дата рождения.
    /// </summary>
    string? Birthday { get; }

    /// <summary>
    ///     Регион.
    /// </summary>
    string? Region { get; }

    /// <summary>
    ///     Адрес.
    /// </summary>
    string? Address { get; }

    /// <summary>
    ///     Диагноз.
    /// </summary>
    string? Diagnosis { get; }

    /// <summary>
    ///     Болезни.
    /// </summary>
    string? Diseases { get; }

    /// <summary>
    ///     Рост.
    /// </summary>
    string? Height { get; }

    /// <summary>
    ///     Вес.
    /// </summary>
    string? Weight { get; }

    /// <summary>
    ///     ФИО родственника.
    /// </summary>
    string? FamilyFullName { get; }

    /// <summary>
    ///     Контактный номер родственника.
    /// </summary>
    string? FamilyPhoneNumber { get; }
}
