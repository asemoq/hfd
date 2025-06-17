using Hfd.Common.Models;

namespace Hfd.Identity.Model;

public class UserFilter : SkipTakeRq
{
    public LoginRole? Role { get; set; }

    public Language? Language { get; set; }
}
