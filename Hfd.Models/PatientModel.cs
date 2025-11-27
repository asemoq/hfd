namespace Hfd.Models;

public class PatientModel : MedPersonModel
{
    public string Scale { get; set; }

    public decimal Imt { get; set; }

    public int Klas { get; set; }

    public bool Dead { get; set; }

    public string Gender { get; set; }

    public string Birthday { get; set; }

    public string Region { get; set; }

    public string Address { get; set; }

    public string Diagnosis { get; set; }

    public string Diseases { get; set; }

    public decimal Height { get; set; }

    public decimal Weight { get; set; }

    public string FamilyFullName { get; set; }

    public string FamilyPhoneNumber { get; set; }
}
