using System.Collections.Generic;
using Hfd.Common.Models;

namespace Hfd.Identity.Model;

/// <summary>
///     User model.
/// </summary>
public class UserInfo
{
    /// <summary>
    ///     Логин пользователя, указанный пользователем.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    ///     ИИН.
    /// </summary>
    public string Iin { get; set; }

    /// <summary>
    ///     ФИО.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    ///     Номер телефона.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    ///     Выбранный язык по умолчанию.
    /// </summary>
    public Language Language { get; set; }

    /// <summary>
    ///     Пароль.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    ///     Выбранные роли.
    /// </summary>
    public IEnumerable<LoginRole> Roles { get; set; }
}
