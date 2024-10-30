using System.Net.Mime;
using Hfd.Base.Contract.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace Hfd.Base.Backend.Controllers;

/// <summary>
///     Управление сервисом.
/// </summary>
[ApiController]
[Route("hfd")]
public class HfdController : ControllerBase
{
	private readonly ILogger<HfdController> _logger;

	/// <summary>
	///     Constructor.
	/// </summary>
	/// <param name="logger"></param>
	public HfdController(ILogger<HfdController> logger)
	{
		_logger = logger;
	}

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
		var error = OverrideErrorDetails();
		var statusCode = StatusCodes.Status418ImATeapot;
		try
		{
			var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>().Error;
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

	protected virtual ErrorDetails OverrideErrorDetails()
	{
		return null;
	}

	protected virtual Task<int?> HandleError(ErrorDetails details, Exception exception, CancellationToken token)
	{
		details.Message = "Внутренняя ошибка сервера.";
		int? statusCode;

		switch (exception)
		{
			case HfdException hfdException:
			{
				details.Message = hfdException.Message;
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
				details.Message = "Ошибка в параметрах";
				details.Details = validationException.Errors.Select(s => s.ToString()).ToArray();
				statusCode = StatusCodes.Status400BadRequest;
				break;
			}
			default:
			{
				details.Details =
					[exception.InnerException?.Message ?? exception.InnerException?.StackTrace ?? string.Empty];
				statusCode = StatusCodes.Status500InternalServerError;
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