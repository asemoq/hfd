using Hfd.Common.Contract.Model;

namespace Hfd.Contract.Model;

public interface IMedPersonInfo : IPersonInfo
{
    /// <summary>
    ///     Ссылка на участок.
    /// </summary>
    long? PlotId { get; set; }
}
