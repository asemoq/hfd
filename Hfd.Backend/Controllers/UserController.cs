using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Hfd.BizLayer.Features;
using Hfd.Common.ApiClient.Model;
using Hfd.Common.Contract.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace Hfd.Backend.Controllers;

/// <inheritdoc />
[SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
[ApiController, Route("user")]
[Display(Order = 2, Name = "Учетные записи")]
public class UserController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    ///     Получить список пользователей.
    /// </summary>
    /// <param name="rq">Параметры запроса.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Список пользователей.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [SwaggerResponse(StatusCodes.Status200OK, "Users")]
    [HttpGet(""), Authorize, Display(Order = 1)]
    [Produces("application/json")]
    public async Task<PageList<UserFull>> GetUsers([FromQuery(Name = "")] UserGetAll.Request rq, CancellationToken token)
        => mapper.Map<IUserFull, UserFull>(await mediator.Send(rq, token));

    /// <summary>
    ///     Создать учетную запись.
    /// </summary>
    /// <param name="rq">Данные учетной записи.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Учетная запись.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "User has been created")]
    [HttpPost(""), Display(Order = 2)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<UserFull> AddUser([FromBody] UserAdd.Request rq, CancellationToken token)
        => mapper.Map<UserFull>(await mediator.Send(rq, token));

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
    public async Task<UserFull> GetUserById([FromRoute] long id, CancellationToken token)
        => mapper.Map<IUserFull, UserFull>(await mediator.Send(UserGet.Request.With(id), token));

    /// <summary>
    ///     Изменить учетную запись.
    /// </summary>
    /// <param name="rq">Данные учетной записи.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Учетная запись.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "User has been updated")]
    [HttpPut, Authorize, Display(Order = 4)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<UserFull> UpdateUser([FromBody] UserUpdate.Request rq, CancellationToken token)
        => mapper.Map<UserFull>(await mediator.Send(rq, token));

    /// <summary>
    ///     Удалить учетную запись.
    /// </summary>
    /// <param name="id">Идентификатор персональных данных.</param>
    /// <param name="token">Токен отмены.</param>
    [SwaggerResponse(StatusCodes.Status204NoContent, "User has been deleted")]
    [HttpDelete("{id:long}"), Authorize, Display(Order = 5)]
    public async Task<ActionResult> DeleteUser([FromRoute] long id, CancellationToken token)
    {
        await mediator.Send(UserDelete.Request.With(id), token);
        return this.NoContent();
    }

    /// <summary>
    ///     Блокировка учетной записи.
    /// </summary>
    /// <param name="id">Идентификатор учетной записи.</param>
    /// <param name="rq">Данные подтверждения.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Учетная запись.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "Блокировка учетной записи выполнена")]
    [HttpPost("{id:long}/block"), Authorize, Display(Order = 9)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<UserFull> BlockUser([FromRoute] long id, [FromBody] UserBlock.Request rq, CancellationToken token)
        => mapper.Map<UserFull>(await mediator.Send(rq.With(id), token));
}
