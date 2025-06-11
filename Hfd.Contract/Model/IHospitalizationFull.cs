using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Опрос.
/// </summary>
public interface IHospitalizationFull : IBaseInfo, IHospitalizationInfo
{
}

public interface IHospitalizationInfo
{
    string Address { get; }

    string DocId { get; }

    DateTime CreationDate { get; }

    long PatientId { get; }
}
