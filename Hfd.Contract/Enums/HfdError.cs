// ReSharper disable InconsistentNaming

namespace Hfd.Contract.Enums;

public enum HfdError
{
    NoError,

    #region [ Nurse ]

    NurseIdIsNotCorrect,
    NurseUserIdIsNotCorrect,

    NurseNotExists,
    NurseUserNotFound,
    NurseUserExist,
    NurseNotFound,

    #endregion

    #region [ Patient ]

    PatientIdIsNotCorrect,
    PatientUserIdIsNotCorrect,

    PatientNotExists,
    PatientNotFound,
    PatientUserNotFound,
    PatientUserExist,

    #endregion

    #region [ Survey ]

    SurveyIdIsNotCorrect,
    SurveyPatientIdIsNotCorrect,

    SurveyAlreadyExists,
    SurveyNotExists,
    SurveyNotFound,

    #endregion

    #region [ ProviderUser ]

    ProviderUserNotExists,
    ProviderUserNotFound,

    #endregion

    #region [ Plot ]

    PlotIdIsNotCorrect,
    PlotNotFound,
    PlotNotExists,

    #endregion
}
