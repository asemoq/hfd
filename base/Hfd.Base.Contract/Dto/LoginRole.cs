using System.ComponentModel.DataAnnotations;

namespace Hfd.Base.Contract.Dto;

public enum LoginRole
{
	/// <summary>
	///     Админ
	/// </summary>
	[Display(Name = "Админ", Description = "Администратор", Order = 1)]
	Admin = 1,

	/// <summary>
	///     Пациент
	/// </summary>
	[Display(Name = "Пациент", Description = "Пациент", Order = 2)]
	Patient = 2,

	/// <summary>
	///     Медсестра
	/// </summary>
	[Display(Name = "Медсестра", Description = "Медсестра", Order = 3)]
	Nurse = 3,

	/// <summary>
	///     Медсестра
	/// </summary>
	[Display(Name = "Психолог", Description = "Психолог", Order = 4)]
	Psychologist = 4,

	/// <summary>
	///     Социальный работник
	/// </summary>
	[Display(Name = "Социальный работник", Description = "Социальный работник", Order = 5)]
	SocialWorker = 5
}