using System.ComponentModel.DataAnnotations;

namespace Hfd.Base.Contract.Enums;

public enum Language
{
	/// <summary>
	///     Русский
	/// </summary>
	[Display(Name = "Русский", Order = 1)] Ru = 1,

	/// <summary>
	///     Казахский
	/// </summary>
	[Display(Name = "Казахский", Order = 2)]
	Kz = 2,

	/// <summary>
	///     Английский
	/// </summary>
	[Display(Name = "Английский", Order = 3)]
	En = 3
}