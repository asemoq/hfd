using Hfd.Common.Models;
using Hfd.Contract.Model;

namespace Hfd.Contract.Dto;

public class NurseModel : BaseModel, INurseModel
{
    /// <inheritdoc />
    public required string PhoneNumber { get; set; }

    /// <inheritdoc />
    public string FullName { get; set; }

    /// <inheritdoc />
    public long? PlotNumber { get; set; }

    /// <inheritdoc />
    public string UserId { get; set; }
}
