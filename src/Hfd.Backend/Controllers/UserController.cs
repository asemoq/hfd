using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Hfd.Base.Backend.Jwt;
using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Model;
using Hfd.BizLayer.Features;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Hfd.Backend.Controllers;

[SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
[ApiController]
[Route("user")]
[Display(Order = 2, Name = "Учетные записи")]
public class UserController : ControllerBase
{
	private readonly IHfdContext _context;
	private readonly ILogger<UserController> _log;
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public UserController(IHfdContext context, IMediator mediator, IMapper mapper, ILogger<UserController> log)
	{
		(_context, _mediator, _mapper, _log) = (context, mediator, mapper, log);
	}

	/// <summary>
	///     Получить список пользователей.
	/// </summary>
	/// <param name="rq">Параметры запроса.</param>
	/// <param name="token">Токен отмены.</param>
	/// <returns>Список пользователей.</returns>
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[SwaggerResponse(StatusCodes.Status200OK, "Users")]
	[HttpGet("")]
	[Authorize]
	[Produces("application/json")]
	[Display(Order = 1)]
	public async Task<PageList<UserFull>> GetUsers([FromQuery(Name = "")] UserGetAll.Request rq,
		CancellationToken token)
	{
		return await _mediator.Send(rq, token);
	}

	/// <summary>
	///     Создать учетную запись.
	/// </summary>
	/// <param name="cm">Данные учетной записи.</param>
	/// <param name="token">Токен отмены.</param>
	/// <returns>Учетная запись.</returns>
	[SwaggerResponse(StatusCodes.Status200OK, "User has been created")]
	[HttpPost("")]
	[Consumes("application/json")]
	[Produces("application/json")]
	[Display(Order = 2)]
	public async Task<UserFull> AddUser([FromBody] UserAdd.Request rq, CancellationToken token)
	{
		return _mapper.Map<UserFull>(await _mediator.Send(rq, token));
	}

	/// <summary>
	///     Получить учетную запись.
	/// </summary>
	/// <param name="id">Идентификатор учетной записи.</param>
	/// <param name="token">Токен отмены.</param>
	/// <returns>Учетная запись.</returns>
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[SwaggerResponse(StatusCodes.Status200OK, "User")]
	[HttpGet("{id}")]
	[Authorize]
	[Produces("application/json")]
	[Display(Order = 3)]
	public async Task<UserFull> GetUser([FromRoute] long id, CancellationToken token)
	{
		return await _mediator.Send(UserGet.Request.With(id), token);
	}

	/// <summary>
	///     Изменить учетную запись.
	/// </summary>
	/// <param name="rq">Данные учетной записи.</param>
	/// <param name="token">Токен отмены.</param>
	/// <returns>Учетная запись.</returns>
	[SwaggerResponse(StatusCodes.Status200OK, "User has been updated")]
	[HttpPut]
	[Authorize]
	[Consumes("application/json")]
	[Produces("application/json")]
	[Display(Order = 4)]
	public async Task<UserFull> UpdateUser([FromBody] UserUpdate.Request rq,
		CancellationToken token)
	{
		return _mapper.Map<UserFull>(await _mediator.Send(rq, token));
	}

	/// <summary>
	///     Удалить учетную запись.
	/// </summary>
	/// <param name="id">Идентификатор персональных данных.</param>
	/// <param name="token">Токен отмены.</param>
	[SwaggerResponse(StatusCodes.Status204NoContent, "User has been deleted")]
	[HttpDelete("{id:long}")]
	[Authorize]
	[Display(Order = 5)]
	public async Task<ActionResult> DeleteUser([FromRoute] long id, CancellationToken token)
	{
		await _mediator.Send(UserDelete.Request.With(id), token);
		return NoContent();
	}

	[SwaggerResponse(StatusCodes.Status200OK, "Login information")]
	[HttpPost("/login")]
	[Consumes("application/json")]
	[Produces("application/json")]
	[Display(Order = 6)]
	public async Task<LoginFull> Login([FromBody] LoginInfo rq)
	{
		var principal = await _context.GetPrincipal(rq.Username, rq.Password);
		if (principal == null)
			throw new UnauthorizedException();

		var handler = _context.Services.GetRequiredService<IJwtHandler>();
		var token = handler.CreateToken(principal, TimeSpan.FromDays(1));

		var rs = new LoginFull
		{
			Usertype = Enum.Parse<LoginRole>(principal.Claims.Single(c => c.Type == ClaimTypes.Role).Value),
			Username = principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value,
			Token = token
		};

		_log.LogInformation($"Пользователю {principal.Identity?.Name} выдан token");
		return rs;
	}

	/// <summary>
	///     Изменить пароль текущего пользователя.
	/// </summary>
	/// <param name="rq">Данные учетной записи.</param>
	/// <param name="token">Токен отмены.</param>
	/// <returns>Асинхронный результат.</returns>
	[SwaggerResponse(StatusCodes.Status204NoContent, "Password changed")]
	[HttpPut("/password")]
	[Authorize]
	[Consumes("application/json")]
	[Display(Order = 7)]
	public async Task<IActionResult> ChangePassword([FromBody] UserPasswordChange.Request rq, CancellationToken token)
	{
		await _mediator.Send(rq, token);
		return NoContent();
	}

	/// <summary>
	///     Блокировка учетной записи.
	/// </summary>
	/// <param name="id">Идентификатор учетной записи.</param>
	/// <param name="rq">Данные подтверждения.</param>
	/// <param name="token">Токен отмены.</param>
	/// <returns>Учетная запись.</returns>
	[SwaggerResponse(StatusCodes.Status200OK, "Блокировка учетной записи выполнена")]
	[HttpPost("{id:long}/block")]
	[Authorize]
	[Consumes("application/json")]
	[Produces("application/json")]
	[Display(Order = 9)]
	public async Task<UserFull> BlockUser([FromRoute] long id, [FromBody] UserBlock.Request rq, CancellationToken token)
	{
		return _mapper.Map<UserFull>(await _mediator.Send(rq.With(id), token));
	}
}