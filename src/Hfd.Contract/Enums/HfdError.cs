// ReSharper disable InconsistentNaming

namespace Hfd.Contract.Enums;

public enum HfdError
{
	HFD_No_Error,

	#region [ Nurse ]

	HFD_Nurse_Id_Is_Not_Correct,
	HFD_Nurse_UserId_Is_Not_Correct,

	HFD_Nurse_Not_Exists,
	HFD_Nurse_User_Not_Found,
	HFD_Nurse_User_Exist,
	HFD_Nurse_Not_Found,

	#endregion

	#region [ Patient ]

	HFD_Patient_Id_Is_Not_Correct,
	HFD_Patient_UserId_Is_Not_Correct,

	HFD_Patient_Not_Exists,
	HFD_Patient_Not_Found,
	HFD_Patient_User_Not_Found,
	HFD_Patient_User_Exist,

	#endregion

	#region [ Survey ]

	HFD_Survey_Id_Is_Not_Correct,
	HFD_Survey_PatientId_Is_Not_Correct,

	HFD_Survey_Already_Exists,
	HFD_Survey_Not_Exists,
	HFD_Survey_Not_Found,

	#endregion

	#region [ ProviderUser ]

	HFD_ProviderUser_Not_Exists,
	HFD_ProviderUser_Not_Found,

	#endregion
}