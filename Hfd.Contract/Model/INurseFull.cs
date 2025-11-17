using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Медсестра.
/// </summary>
public interface INurseFull : IBaseInfo, INurseInfo
{
}

public interface INurseInfo : IMedPersonInfo
{
}
