// ReSharper disable InconsistentNaming

namespace Hfd.Base.Contract.Exceptions;

public enum HfdBaseError
{
	No_Error,

	#region [ User ]

	User_NameOrPassword_Not_Correct,
	User_Not_Found,
	User_Blocked,
	User_Already_Exists,
	User_Usertype_Not_Correct,
	User_Language_Not_Correct,
	User_Username_Cannot_Be_Empty,
	User_Iin_Cannot_Be_Empty,
	User_FullName_Cannot_Be_Empty,
	User_Password_Cannot_Be_Empty,
	User_Password_Lenght_Not_Correct,
	User_BlockedReason_Cannot_Be_Empty,

	#endregion
}