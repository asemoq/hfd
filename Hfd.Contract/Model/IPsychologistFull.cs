using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Опрос.
/// </summary>
public interface IPsychologistFull : IBaseInfo, IPsychologistInfo
{
}

public interface IPsychologistInfo
{
    string? PhoneNumber { get; }

    string FullName { get; }

    long? PlotNumber { get; }

    string UserId { get; }
}
