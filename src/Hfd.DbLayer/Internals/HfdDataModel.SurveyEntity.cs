using System.ComponentModel.DataAnnotations.Schema;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hfd.DbLayer.Internals;

///// <summary>
///// Data model part.
///// </summary>
public partial class HfdDataModel
{
	/// <inheritdoc cref="SurveyEntity" />
	/// .
	public virtual DbSet<SurveyEntity> Surveys { get; set; }
}

/// <summary>
///     Entity.
/// </summary>
[Table("survey")]
public class SurveyEntity : HfdEntityBase, ISurveyFull
{
	/// <inheritdoc />
	public DyspneaType Dyspnea { get; set; }

	/// <inheritdoc />
	public double AdValue { get; set; }

	/// <inheritdoc />
	[NotMapped]
	public AdType Ad => AdValue switch
	{
		>= 120 => AdType.Normal,
		< 120 and > 100 => AdType.FirstHypotension,
		<= 100 => AdType.SecondHypotension,
		_ => throw new ArgumentOutOfRangeException()
	};

	/// <inheritdoc />
	public double WeightValue { get; set; }

	/// <inheritdoc />
	[NotMapped]
	public WeightType Weight => WeightType.Normal;

	/// <inheritdoc />
	public EdemaType Edema { get; set; }

	/// <inheritdoc />
	public double HeartRateValue { get; set; }

	/// <inheritdoc />
	[NotMapped]
	public HeartRateType HeartRate => HeartRateValue switch
	{
		>= 90 => HeartRateType.TachycardiaOrBradycardia,
		< 90 and > 60 => HeartRateType.Normal,
		<= 60 => HeartRateType.TachycardiaOrBradycardia,
		_ => throw new ArgumentOutOfRangeException()
	};

	/// <inheritdoc />
	public PositionInBedType PositionInBed { get; set; }

	/// <inheritdoc />
	public long PatientId { get; set; }

	/// <inheritdoc />
	public DateTime Date { get; set; }

	#region [ Configuration ]

	/// <inheritdoc />
	public class Configuration : IEntityTypeConfiguration<SurveyEntity>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<SurveyEntity> entity)
		{
			entity
				.HasKey(x => x.Id);

			entity
				.Property(x => x.Id)
				.ValueGeneratedOnAdd();
		}
	}

	#endregion

	#region Navigation

	/// <summary>
	///     Patient.
	/// </summary>
	[ForeignKey(nameof(PatientId))]
	[InverseProperty(nameof(Internals.PatientEntity.SurveyEntities))]
	public PatientEntity PatientEntity { get; set; }

	#endregion
}