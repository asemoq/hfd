using System.ComponentModel.DataAnnotations;
using Hfd.BizLayer.Features;
using Microsoft.AspNetCore.Authorization;

namespace Hfd.Backend.Controllers;

/// <summary>
///     Управление медсестрами.
/// </summary>
[ApiController]
[Route("nurse")]
[Authorize]
[Display(Order = 7, Name = "Медсестры")]
public class NurseController : ControllerBase
{
	private readonly IMediator _mediator;

	/// <summary>
	///     Constructor.
	/// </summary>
	/// <param name="mediator">Command processor.</param>
	public NurseController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	///     Получить список всех медсестер.
	/// </summary>
	/// <returns>Список медсестер.</returns>
	[HttpGet]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<ObjectResult> GetAll([FromQuery] NurseGetAll.Request request, CancellationToken token)
	{
		return Ok(await _mediator.Send(request, token));
	}

	/// <summary>
	///     Получить медсестру по идентификатору.
	/// </summary>
	/// <returns>Медсестра.</returns>
	[HttpGet("{id:long}")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<ObjectResult> GetById([FromRoute] long id, CancellationToken token)
	{
		return Ok(await _mediator.Send(new NurseGetById.Request { NurseId = id }, token));
	}

	/// <summary>
	///     Добавить медсестру.
	/// </summary>
	/// <returns>Медсестра.</returns>
	[Display(Order = 1)]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[HttpPost]
	[Produces("application/json")]
	public async Task<ObjectResult> Add(NurseAdd.Request request, CancellationToken token)
	{
		return Ok(await _mediator.Send(request, token));
	}

	/// <summary>
	///     Редактировать медсестру.
	/// </summary>
	/// <returns>Медсестра.</returns>
	[Display(Order = 1)]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[HttpPut]
	[Produces("application/json")]
	public async Task<ObjectResult> Edit(NurseEdit.Request request, CancellationToken token)
	{
		return Ok(await _mediator.Send(request, token));
	}

	/// <summary>
	///     Удалить медсестру.
	/// </summary>
	/// <returns>Результат.</returns>
	[HttpDelete("{id:long}")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<ObjectResult> Delete([FromRoute] long id, CancellationToken token)
	{
		return Ok(await _mediator.Send(new NurseDelete.Request { NurseId = id }, token));
	}
}