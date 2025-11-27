using Hfd.Common.Contract.Model;
using Hfd.Contract.Enums;

namespace Hfd.Contract.Model;

/// <summary>
///     Пациент.
/// </summary>
public interface IPatientFull : IBaseInfo, IPatientInfo
{
}

public interface IPatientInfo : IMedPersonInfo
{
    /// <summary>
    ///     Рост.
    /// </summary>
    decimal Height { get; }

    /// <summary>
    ///     Вес.
    /// </summary>
    decimal Weight { get; }

    /// <summary>
    ///     Индекс массы тела.
    /// </summary>
    decimal Imt { get; }

    /// <summary>
    ///     Шкала.
    /// </summary>
    string? Scale { get; }

    int Klas { get; }

    bool Dead { get; }

    /// <summary>
    ///     Пол.
    /// </summary>
    Gender Gender { get; }

    /// <summary>
    ///     Дата рождения.
    /// </summary>
    DateTime Birthday { get; }

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
    ///     ФИО родственника.
    /// </summary>
    string FamilyFullName { get; }

    /// <summary>
    ///     Контактный номер родственника.
    /// </summary>
    string FamilyPhoneNumber { get; }
}
