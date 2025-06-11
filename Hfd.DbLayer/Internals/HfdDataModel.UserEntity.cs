using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Common.Contract.Enums;
using Hfd.Common.Contract.Model;
using Hfd.Common.DbLayer.Internals;
using Microsoft.EntityFrameworkCore;

namespace Hfd.DbLayer.Internals;

///// <summary>
///// Data model part.
///// </summary>
public partial class HfdDataModel
{
    /// <inheritdoc cref="UserEntity" />
    /// .
    public virtual DbSet<UserEntity> Users { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("user")]
public class UserEntity : EntityBase, IUserFull
{
    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    public string? UniqueId { get; set; }

    /// <inheritdoc />
    public LoginRole Usertype { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    public required string Username { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    public required string Password { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    public string? FullName { get; set; }

    /// <inheritdoc />
    public bool Blocked { get; set; }

    /// <inheritdoc />
    public Language Language { get; set; }

    #region Navigation

    public PatientEntity? PatientEntity { get; set; }

    public NurseEntity? NurseEntity { get; set; }

    public PsychologistEntity? PsychologistEntity { get; set; }

    public SocialWorkerEntity? SocialWorkerEntity { get; set; }

    #endregion
}
