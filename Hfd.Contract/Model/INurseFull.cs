using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Медсестра.
/// </summary>
public interface INurseFull : INurseModel
{
}

public interface INurseModel : IBaseInfo, INurseInfo
{
}

public interface INurseInfo
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
    string UserId { get; }
}
