// ReSharper disable InconsistentNaming

namespace Hfd.Contract.Enums;

public enum HfdError
{
	HFD_No_Error,

	#region [ User ]

	HFD_User_Id_Is_Not_Correct,

	HFD_User_Not_Exists,
	HFD_User_Not_Found,

	#endregion

	#region [ Nurse ]

	HFD_Nurse_Id_Is_Not_Correct,

	HFD_Nurse_Not_Exists,
	HFD_Nurse_Not_Found,

	#endregion

	#region [ Patient ]

	HFD_Patient_Not_Exists,
	HFD_Patient_Not_Found,

	#endregion

	#region [ PatientCard ]

	HFD_PatientCard_Not_Exists,
	HFD_PatientCard_Not_Found,

	#endregion

	#region [ Survey ]

	HFD_Survey_Already_Exists,
	HFD_Survey_Not_Exists,
	HFD_Survey_Not_Found,

	#endregion

	#region [ ProviderUser ]

	HFD_ProviderUser_Not_Exists,
	HFD_ProviderUser_Not_Found,

	#endregion
}