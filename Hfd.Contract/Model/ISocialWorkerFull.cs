using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Опрос.
/// </summary>
public interface ISocialWorkerFull : IBaseInfo, ISocialWorkerInfo
{
}

public interface ISocialWorkerInfo : IPersonInfo
{
}
