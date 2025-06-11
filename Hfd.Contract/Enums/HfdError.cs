// ReSharper disable InconsistentNaming

namespace Hfd.Contract.Enums;

public enum HfdError
{
    HFDNoError,

    #region [ Nurse ]

    HFDNurseIdIsNotCorrect,
    HFDNurseUserIdIsNotCorrect,
    HFDNursePhoneNumberIsNotValid,

    HFDNurseNotExists,
    HFDNurseUserNotFound,
    HFDNurseUserExist,
    HFDNurseNotFound,

    #endregion

    #region [ Patient ]

    HFDPatientIdIsNotCorrect,
    HFDPatientUserIdIsNotCorrect,
    HFDPatientPhoneNumberIsNotValid,

    HFDPatientNotExists,
    HFDPatientNotFound,
    HFDPatientUserNotFound,
    HFDPatientUserExist,

    #endregion

    #region [ Survey ]

    HFDSurveyIdIsNotCorrect,
    HFDSurveyPatientIdIsNotCorrect,

    HFDSurveyAlreadyExists,
    HFDSurveyNotExists,
    HFDSurveyNotFound,

    #endregion

    #region [ ProviderUser ]

    HFDProviderUserNotExists,
    HFDProviderUserNotFound,

    #endregion
}
