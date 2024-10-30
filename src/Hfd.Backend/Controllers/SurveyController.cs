using System.ComponentModel.DataAnnotations;
using Hfd.BizLayer.Features;
using Microsoft.AspNetCore.Authorization;

namespace Hfd.Backend.Controllers;

/// <summary>
///     Управление опросами.
/// </summary>
[ApiController]
[Route("survey")]
[Authorize]
[Display(Order = 7, Name = "Опросы")]
public class SurveyController : ControllerBase
{
	private readonly IMediator _mediator;

	/// <summary>
	///     Constructor.
	/// </summary>
	/// <param name="mediator">Command processor.</param>
	public SurveyController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	///     Получить список всех опросов.
	/// </summary>
	/// <returns>Список опросов.</returns>
	[HttpGet]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<ObjectResult> GetAll([FromQuery] SurveyGetAll.Request request, CancellationToken token)
	{
		return Ok(await _mediator.Send(request, token));
	}

	/// <summary>
	///     Получить опрос по идентификатору.
	/// </summary>
	/// <returns>Опрос.</returns>
	[HttpGet("{id:long}")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<ObjectResult> GetById([FromRoute] long id, CancellationToken token)
	{
		return Ok(await _mediator.Send(new SurveyGetById.Request { SurveyId = id }, token));
	}

	/// <summary>
	///     Заполнить опрос.
	/// </summary>
	/// <returns>Опрос.</returns>
	[Display(Order = 1)]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[HttpPost]
	[Produces("application/json")]
	public async Task<ObjectResult> Add(SurveyAdd.Request request, CancellationToken token)
	{
		return Ok(await _mediator.Send(request, token));
	}

	/// <summary>
	///     Редактировать опрос.
	/// </summary>
	/// <returns>Опрос.</returns>
	[Display(Order = 1)]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[HttpPut]
	[Produces("application/json")]
	public async Task<ObjectResult> Edit(SurveyEdit.Request request, CancellationToken token)
	{
		return Ok(await _mediator.Send(request, token));
	}
}