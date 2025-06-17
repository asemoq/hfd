using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Models;
using Hfd.Identity.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Hfd.Identity.Controllers;

/// <inheritdoc />
[SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
[ApiController, Route("user")]
[Display(Order = 2, Name = "Учетные записи")]
[Authorize(Roles = "Administrator")]
public class UserController(UserManager<HfdIdentityUser> userManager) : ControllerBase
{
    /// <summary>
    ///     Получить список пользователей.
    /// </summary>
    /// <param name="filter">Параметры запроса.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Список пользователей.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [SwaggerResponse(StatusCodes.Status200OK, "Users")]
    [HttpGet(""), Authorize, Display(Order = 1)]
    [Produces("application/json")]
    public async Task<PageList<HfdIdentityUser>> GetUsers([FromQuery(Name = "")] UserFilter filter, CancellationToken token)
    {
        var total = await userManager.Users.CountAsync(cancellationToken: token);
        var skip = Math.Min(total, filter.Skip);
        var take = Math.Min(total - filter.Skip, filter.Take == 0 ? total : filter.Take);
        return new PageList<HfdIdentityUser>
        {
            Total = total,
            First = skip,
            Items = take > 0
                ? await userManager.Users.AsNoTracking().Skip(skip).Take(take).ToListAsync(cancellationToken: token)
                : [],
        };
    }

    /// <summary>
    ///     Создать учетную запись.
    /// </summary>
    /// <param name="info">Данные учетной записи.</param>
    /// <returns>Учетная запись.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "User has been created")]
    [HttpPost(""), Display(Order = 2)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<HfdIdentityUser> AddUser([FromBody] UserInfo info)
    {
        var user = new HfdIdentityUser() { Iin = info.Iin, FullName = info.FullName };

        var result = await userManager.CreateAsync(user, info.Password!);
        if (result.Succeeded)
        {
            return user;
        }

        throw new HfdException(result.Errors.Select(x => x.Description).ToArray());
    }

    /// <summary>
    ///     Получить учетную запись.
    /// </summary>
    /// <param name="id">Идентификатор учетной записи.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Учетная запись.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [SwaggerResponse(StatusCodes.Status200OK, "User")]
    [HttpGet("{id}"), Authorize, Display(Order = 3)]
    [Produces("application/json")]
    public async Task<HfdIdentityUser> GetUserById([FromRoute] string id, CancellationToken token)
        => await userManager.FindByIdAsync(id) ?? throw new UserNotFoundException();

    /// <summary>
    ///     Изменить учетную запись.
    /// </summary>
    /// <param name="id">Идентификатор учетной записи.</param>
    /// <param name="info">Данные учетной записи.</param>
    /// <returns>Учетная запись.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "User has been updated")]
    [HttpPut("{id}"), Authorize, Display(Order = 4)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<HfdIdentityUser> UpdateUser([FromRoute] string id, [FromBody] UserInfo info)
    {
        var user = await userManager.FindByIdAsync(id) ?? throw new UserNotFoundException();
        user.FullName = info.FullName;
        user.PhoneNumber = info.PhoneNumber;
        user.Language = info.Language;
        user.Iin = info.Iin;
        await userManager.UpdateAsync(user);
        foreach (var role in info.Roles)
        {
            await userManager.AddToRoleAsync(user, role.ToString());
        }

        return user;
    }

    /// <summary>
    ///     Удалить учетную запись.
    /// </summary>
    /// <param name="id">Идентификатор персональных данных.</param>
    /// <param name="token">Токен отмены.</param>
    [SwaggerResponse(StatusCodes.Status204NoContent, "User has been deleted")]
    [HttpDelete("{id}"), Authorize, Display(Order = 5)]
    public async Task<ActionResult> DeleteUser([FromRoute] string id, CancellationToken token)
    {
        var user = await userManager.FindByIdAsync(id) ?? throw new UserNotFoundException();
        await userManager.DeleteAsync(user);
        return this.NoContent();
    }
}
