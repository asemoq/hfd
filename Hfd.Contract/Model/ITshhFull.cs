using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

public interface ITshhFull : IBaseInfo, ITshhInfo
{
}

public interface ITshhInfo
{
    long Klass { get; }

    long Distance { get; }

    long PatientId { get; }

    DateTime CreationDate { get; }
}
