using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Опрос.
/// </summary>
public interface IPsychologistFull : IPersonInfo, IPsychologistInfo
{
}

public interface IPsychologistInfo
{
    long? PlotNumber { get; }
}
