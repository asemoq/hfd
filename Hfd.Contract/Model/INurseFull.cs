using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Медсестра.
/// </summary>
public interface INurseFull : IPersonInfo, INurseInfo
{
}

public interface INurseInfo
{
    /// <summary>
    ///     Номер участка
    /// </summary>
    long? PlotNumber { get; }
}
