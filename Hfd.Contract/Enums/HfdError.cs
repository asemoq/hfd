// ReSharper disable InconsistentNaming

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
    SurveyPatientId,

    #endregion

    #region [ ProviderUser ]

    ProviderUser,

    #endregion

    #region [ Plot ]

    Plot,
    PlotId,

    #endregion
}
