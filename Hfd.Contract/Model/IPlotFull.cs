using System.Collections.Generic;
using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

/// <summary>
///     Участок.
/// </summary>
public interface IPlotFull : IBaseInfo, IPlotInfo
{
    IEnumerable<INurseFull> Nurses { get; }

    IEnumerable<IPatientFull> Patients { get; }

    IEnumerable<IPsychologistFull> Psychologists { get; }

    IEnumerable<ISocialWorkerFull> SocialWorkers { get; }
}

public interface IPlotInfo
{
    int Number { get; }

    string Area { get; }
}
