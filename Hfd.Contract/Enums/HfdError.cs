namespace Hfd.Contract.Enums;

public enum HfdError
{
    #region [ Nurse ]

    Nurse,
    NurseId,
    NurseUser,
    NurseUserId,

    #endregion

    #region [ Patient ]

    Patient,
    PatientId,
    PatientUser,
    PatientUserId,
    PatientWeight,
    PatientHeight,

    #endregion

    #region [ Survey ]

    Survey,
    SurveyId,
    SurveyAd,
    SurveyWeight,
    SurveyHeartRate,
    SurveyPatientId,

    #endregion

    #region [ Plot ]

    Plot,
    PlotId,

    #endregion
}
