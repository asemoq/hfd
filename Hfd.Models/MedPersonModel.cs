using Hfd.Common.Models;

namespace Hfd.Models;

public class MedPersonModel : PersonModel
{
    /// <summary>
    ///     Identifier of the plot.
    /// </summary>
    public long? PlotId { get; set; }

    /// <summary>
    /// Plot model.
    /// </summary>
    public PlotModel? Plot { get; set; }
}
