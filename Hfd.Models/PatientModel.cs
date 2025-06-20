using Hfd.Common.Models;

namespace Hfd.Models;

public class PatientModel : PersonModel
{
    public string Scale { get; set; }

    public string Imt { get; set; }

    public int Klas { get; set; }

    public bool Dead { get; set; }

    public string Gender { get; set; }

    public string Birthday { get; set; }

    public string Region { get; set; }

    public string Address { get; set; }

    public string Diagnosis { get; set; }

    public string Diseases { get; set; }

    public string Height { get; set; }

    public string Weight { get; set; }

    public string FamilyFullName { get; set; }

    public string FamilyPhoneNumber { get; set; }
}
