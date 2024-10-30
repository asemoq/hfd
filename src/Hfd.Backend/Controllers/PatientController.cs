using System.ComponentModel.DataAnnotations;
using Hfd.BizLayer.Features;
using Microsoft.AspNetCore.Authorization;

namespace Hfd.Backend.Controllers;

/// <summary>
///     Управление пациентами.
/// </summary>
[ApiController]
[Route("patient")]
[Authorize]
[Display(Order = 7, Name = "Пациенты")]
public class PatientController : ControllerBase
{
	private readonly IMediator _mediator;

	/// <summary>
	///     Constructor.
	/// </summary>
	/// <param name="mediator">Command processor.</param>
	public PatientController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	///     Получить список всех пациентов.
	/// </summary>
	/// <returns>Список пациентов.</returns>
	[HttpGet]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<ObjectResult> GetAll([FromQuery] PatientGetAll.Request request, CancellationToken token)
	{
		return Ok(await _mediator.Send(request, token));
	}

	/// <summary>
	///     Получить пациента по идентификатору.
	/// </summary>
	/// <returns>Пациент.</returns>
	[HttpGet("{id:long}")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<ObjectResult> GetById([FromRoute] long id, CancellationToken token)
	{
		return Ok(await _mediator.Send(new PatientGetById.Request { PatientId = id }, token));
	}

	/// <summary>
	///     Добавить пациента.
	/// </summary>
	/// <returns>Пациент.</returns>
	[Display(Order = 1)]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[HttpPost]
	[Produces("application/json")]
	public async Task<ObjectResult> Add(PatientAdd.Request request, CancellationToken token)
	{
		return Ok(await _mediator.Send(request, token));
	}

	/// <summary>
	///     Редактировать пациента.
	/// </summary>
	/// <returns>Пациент.</returns>
	[Display(Order = 1)]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[HttpPut]
	[Produces("application/json")]
	public async Task<ObjectResult> Edit(PatientEdit.Request request, CancellationToken token)
	{
		return Ok(await _mediator.Send(request, token));
	}

	/// <summary>
	///     Удалить пациента.
	/// </summary>
	/// <returns>Результат.</returns>
	[HttpDelete("{id:long}")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<ObjectResult> Delete([FromRoute] long id, CancellationToken token)
	{
		return Ok(await _mediator.Send(new PatientDelete.Request { PatientId = id }, token));
	}
}