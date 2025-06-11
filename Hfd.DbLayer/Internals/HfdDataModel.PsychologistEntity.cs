using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Common.DbLayer.Internals;
using Hfd.Contract.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hfd.DbLayer.Internals;

///// <summary>
///// Data model part.
///// </summary>
public partial class HfdDataModel
{
    /// <inheritdoc cref="PsychologistEntity" />
    /// .
    public virtual DbSet<PsychologistEntity> Psychologists { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("psychologist")]
public class PsychologistEntity : EntityBase, IPsychologistFull
{
    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    public string? PhoneNumber { get; set; }

    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    public string? FullName { get; set; }

    /// <inheritdoc />
    public long? PlotNumber { get; set; }

    /// <inheritdoc />
    public long UserId { get; set; }

    #region Navigation

    public UserEntity? UserEntity { get; set; }

    #endregion
}
