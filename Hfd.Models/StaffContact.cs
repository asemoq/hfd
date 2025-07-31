using System.Collections.Generic;

namespace Hfd.Models;

public class StaffContact
{
    /// <summary>
    /// Nurses.
    /// </summary>
    public IReadOnlyCollection<Contact> Nurses { get; set; }

    /// <summary>
    /// Psychologists.
    /// </summary>
    public IReadOnlyCollection<Contact> Psychologists { get; set; }

    /// <summary>
    /// Social workers.
    /// </summary>
    public IReadOnlyCollection<Contact> SocialWorkers { get; set; }
}

public class Contact
{
    /// <summary>
    /// Full name.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Phone number.
    /// </summary>
    public string PhoneNumber { get; set; }
}
