using System;

namespace Hfd.Models;

public class PatientModel : MedPersonModel
{
    public string Scale { get; set; }

    public double Imt { get; set; }

    public int Klas { get; set; }

    public bool Dead { get; set; }

    public string Gender { get; set; }

    public DateTime Birthday { get; set; }

    public string Region { get; set; }

    public string Address { get; set; }

    public string Diagnosis { get; set; }

    public string Diseases { get; set; }

    public double Height { get; set; }

    public double Weight { get; set; }

    public string FamilyFullName { get; set; }

    public string FamilyPhoneNumber { get; set; }

    public int Age => DateTime.Today.Year
                      - this.Birthday.Year
                      - 1
                      + ((DateTime.Today.Month > this.Birthday.Month
                          || (DateTime.Today.Month == this.Birthday.Month && DateTime.Today.Day >= this.Birthday.Day))
                          ? 1
                          : 0);
}
