using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Security.Claims;
using Hfd.Base.Backend.Jwt;
using Hfd.Base.BizLayer.Features;
using Hfd.Base.Contract.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ValidationException = FluentValidation.ValidationException;

namespace Hfd.Base.Backend.Controllers;

/// <summary>
///     Управление сервисом.
/// </summary>
[SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
[ApiController, Route("hfd")]
[Display(Order = 1, Name = "Управление сервисом")]
public class HfdController : ControllerBase
{
	private readonly ILogger<HfdController> _logger;
	private readonly IBackendContext _context;

	/// <summary>
	///     Constructor.
	/// </summary>
	/// <param name="context"></param>
	/// <param name="logger"></param>
	public HfdController(IBackendContext context, ILogger<HfdController> logger)
	{
		(_context, _logger) = (context, logger);
	}

	/// <summary>
	/// Авторизация
	/// </summary>
	/// <param name="rq"></param>
	/// <returns></returns>
	/// <exception cref="UnauthorizedException"></exception>
	[SwaggerResponse(StatusCodes.Status200OK, "Login information")]
	[HttpPost("/login"), Display(Order = 6)]
	[Consumes("application/json")]
	[Produces("application/json")]
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

		_logger.LogInformation($"Пользователю {principal.Identity?.Name} выдан token");
		return rs;
	}

	/// <summary>
	///     Изменить пароль текущего пользователя.
	/// </summary>
	/// <param name="rq">Данные учетной записи.</param>
	/// <param name="token">Токен отмены.</param>
	/// <returns>Асинхронный результат.</returns>
	[SwaggerResponse(StatusCodes.Status204NoContent, "Password changed")]
	[HttpPut("/password"), Authorize, Display(Order = 7)]
	[Consumes("application/json")]
	public async Task<IActionResult> ChangePassword([FromBody] UserPasswordChange.Request rq, CancellationToken token)
	{
		await _context.Services.GetRequiredService<IMediator>().Send(rq, token);
		return NoContent();
	}

	/// <summary>
	/// Получить учетную запись текущего пользователя.
	/// </summary>
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[ProducesResponseType(typeof(UserFull), StatusCodes.Status200OK)]
	[HttpGet("/self"), Authorize]
	[Produces("application/json")]
	[Display(Order = 5)]
	public Task<UserFull> GetUiUserSelf(CancellationToken token = default)
		=> Task.FromResult(_context.BizContext.UserFull);

	/// <summary>
	///     Обработчик ошибок - генерирует стандартизованное описание ошибки.
	/// </summary>
	/// <param name="token">Cancellation token.</param>
	/// <returns>Описание ошибки.</returns>
	[ApiExplorerSettings(IgnoreApi = true)]
	[Route("/api/error")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<IActionResult> Error(CancellationToken token)
	{
		var error = OverrideErrorDetails() ?? new ErrorDetails();
		var statusCode = StatusCodes.Status418ImATeapot;
		try
		{
			var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;
			statusCode =
				await HandleError(error, exception, token)
				?? StatusCodes.Status400BadRequest;
		}
		catch (Exception ex)
		{
			error.Message = "Ошибка обработки ошибки.";
			error.Details = new[] { ex.Message };
		}

		var result = new ObjectResult(error) { StatusCode = statusCode };
		result.ContentTypes.Add(MediaTypeNames.Application.Json);
		return result;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	protected virtual ErrorDetails? OverrideErrorDetails()
	{
		return null;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="details"></param>
	/// <param name="exception"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	protected virtual Task<int?> HandleError(ErrorDetails details, Exception exception, CancellationToken token)
	{
		details.Message = "Internal Server Error.";
		int? statusCode;

		switch (exception)
		{
			case HfdException hfdException:
			{
				details.Message = hfdException.Message;
				details.Details = hfdException.Details.ToArray();
				statusCode = hfdException.GetHttpStatus();
				break;
			}
			case InvalidOperationException invalidOperationException:
			{
				details.Message = invalidOperationException.Message;
				statusCode = StatusCodes.Status405MethodNotAllowed;
				break;
			}
			case ValidationException validationException:
			{
				details.Message = "Validation error";
				details.Details = validationException.Errors.Select(s => s.ToString()).ToArray();
				statusCode = StatusCodes.Status400BadRequest;
				break;
			}
			default:
			{
				details.Details =
					[exception?.InnerException?.Message ?? exception?.InnerException?.StackTrace ?? string.Empty];
				statusCode = StatusCodes.Status500InternalServerError;
				break;
			}
		}

		switch (exception?.InnerException)
		{
			case HfdException hfdException:
			{
				details.Message = hfdException.Message;
				details.Details = hfdException.Details.ToArray();
				statusCode = hfdException.GetHttpStatus();
				break;
			}
		}

		_logger.LogError(exception, $"Обработана ошибка: {exception?.Message}");
		return Task.FromResult(statusCode);
	}

	/// <summary>
	///     Обобщенное описание случившейся ошибки.
	/// </summary>
	protected class ErrorDetails
	{
		/// <summary>
		///     Текст ошибки.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		///     Детали ошибки.
		/// </summary>
		public string[] Details { get; set; }
	}
}