using Hfd.Common.Models;

namespace Hfd.Models;

public class UserModel : UserFull
{
    /// <summary>
    /// Patient model.
    /// </summary>
    public PatientModel? Patient { get; set; }

    /// <summary>
    /// Nurse model.
    /// </summary>
    public NurseModel? Nurse { get; set; }
}
