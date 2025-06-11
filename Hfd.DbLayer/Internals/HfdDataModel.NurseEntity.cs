using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Common.Contract.Model;
using Hfd.Common.DbLayer.Internals;
using Hfd.Contract.Model;

namespace Hfd.DbLayer.Internals;

///// <summary>
///// Data model part.
///// </summary>
public partial class HfdDataModel
{
    /// <inheritdoc cref="NurseEntity" />
    /// .
    public virtual DbSet<NurseEntity> Nurses { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("nurse")]
public class NurseEntity : EntityBase, INurseFull
{
    /// <inheritdoc />
    [MaxLength(int.MaxValue)]
    public required string PhoneNumber { get; set; }

    /// <inheritdoc />
    public long? PlotNumber { get; set; }

    /// <inheritdoc />
    public long UserId { get; set; }

    /// <inheritdoc />
    [NotMapped]
    public IUserFull User => this.UserEntity!;

    #region Navigation

    public UserEntity? UserEntity { get; set; }

    [InverseProperty(nameof(PatientEntity.NurseEntity))]
    public virtual ICollection<PatientEntity> PatientEntities { get; init; } = new HashSet<PatientEntity>();

    #endregion
}
