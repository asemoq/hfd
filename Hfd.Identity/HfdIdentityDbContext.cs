using Hfd.Common.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hfd.Identity;

/// <inheritdoc />
public class HfdIdentityDbContext(DbContextOptions<HfdIdentityDbContext> options)
    : IdentityDbContext<HfdIdentityUser>(options)
{
}
