using System.Runtime.Serialization;

namespace Hfd.Contract.Enums;

public enum FileType
{
    None = 0,
    Audio = 10,
    Document = 20,
    Photo = 30,
    Voice = 40,
    Video = 50,
}

public enum DyspneaType
{
    /// <summary>
    ///     Нет одышки
    /// </summary>
    No = 0,

    /// <summary>
    ///     При нагрузке
    /// </summary>
    AfterActivity = 1,

    /// <summary>
    ///     В покое
    /// </summary>
    AtRest = 2,
}

public enum AdType
{
    /// <summary>
    ///     Норма
    /// </summary>
    Normal = 0,

    /// <summary>
    ///     Гипотония уровень 1
    /// </summary>
    FirstHypotension = 1,

    /// <summary>
    ///     Гипотония уровень 2
    /// </summary>
    SecondHypotension = 2,
}

public enum WeightType
{
    /// <summary>
    ///     Норма
    /// </summary>
    Normal = 0,

    /// <summary>
    ///     Набор веса
    /// </summary>
    WeightGain = 1,
}

public enum HeartRateType
{
    /// <summary>
    ///     Норма
    /// </summary>
    Normal = 0,

    /// <summary>
    ///     Тахикардия или брадикардия
    /// </summary>
    TachycardiaOrBradycardia = 1,
}

public enum EdemaType
{
    /// <summary>
    ///     Нет отеков
    /// </summary>
    No = 0,

    /// <summary>
    ///     Выраженные отеки
    /// </summary>
    Pronounced = 1,

    /// <summary>
    ///     Тяжелые отеки
    /// </summary>
    Severe = 2,

    /// <summary>
    ///     Асцит
    /// </summary>
    Ascites = 3,
}

public enum PositionInBedType
{
    /// <summary>
    ///     Горизонтально
    /// </summary>
    Horizontally = 0,

    /// <summary>
    ///     Приподнятым головным концом
    /// </summary>
    RaisedHeadEnd = 1,

    /// <summary>
    ///     Просыпается от удушья
    /// </summary>
    WakesUpFromSuffocation = 2,

    /// <summary>
    ///     Сидя
    /// </summary>
    Sitting = 3,
}

public enum Indicator
{
    /// <summary>
    ///     Вес
    /// </summary>
    [EnumMember(Value = "weight")] Weight = 0,

    /// <summary>
    ///     Давление
    /// </summary>
    [EnumMember(Value = "ad")] Ad = 1,

    /// <summary>
    ///     Пульс
    /// </summary>
    [EnumMember(Value = "heart_rate")] HeartRate = 2,
}

public enum ZoneType
{
    /// <summary>
    ///     Зеленая зона
    /// </summary>
    Green = 0,

    /// <summary>
    ///     Желтая зона
    /// </summary>
    Yellow = 1,

    /// <summary>
    ///     Красная зона
    /// </summary>
    Red = 2,
}

public enum DateType
{
    /// <summary>
    ///     Утро
    /// </summary>
    // ReSharper disable once InconsistentNaming
    AM,

    /// <summary>
    ///     Вечер
    /// </summary>
    // ReSharper disable once InconsistentNaming
    PM,
}

public enum Gender
{
    Male,
    Female,
}
