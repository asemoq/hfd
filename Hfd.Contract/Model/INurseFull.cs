using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Медсестра.
/// </summary>
public interface INurseFull : INurseModel
{
    /// <summary>
    ///     Пользователь.
    /// </summary>
    IUserFull User { get; }
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
    ///     Номер участка
    /// </summary>
    long? PlotNumber { get; }

    /// <summary>
    ///     Ссылка на пользователя
    /// </summary>
    long UserId { get; }
}
