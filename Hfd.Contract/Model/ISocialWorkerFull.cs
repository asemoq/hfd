using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Опрос.
/// </summary>
public interface ISocialWorkerFull : IPersonInfo, ISocialWorkerInfo
{
}

public interface ISocialWorkerInfo
{
    long? PlotNumber { get; }
}
