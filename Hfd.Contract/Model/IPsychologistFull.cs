using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Опрос.
/// </summary>
public interface IPsychologistFull : IBaseInfo, IPsychologistInfo
{
}

public interface IPsychologistInfo : IPersonInfo
{
}
