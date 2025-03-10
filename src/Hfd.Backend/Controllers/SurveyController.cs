using System.ComponentModel.DataAnnotations;
using Hfd.Base.ApiClient.Model;
using Hfd.BizLayer.Features;
using Hfd.Contract.Dto;
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
	public SurveyController(IMediator mediator) => _mediator = mediator;

	/// <summary>
	///     Получить список всех опросов.
	/// </summary>
	/// <returns>Список опросов.</returns>
	[HttpGet]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<PageList<SurveyModel>> GetAll([FromQuery] SurveyGetAll.Request request, CancellationToken token)
		=> await _mediator.Send(request, token);

	/// <summary>
	///     Получить график.
	/// </summary>
	/// <returns>График.</returns>
	[HttpGet("summary")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<SummaryModel> GetSummary([FromQuery] SurveyGetBySummary.Request request, CancellationToken token)
		=> await _mediator.Send(request, token);

	/// <summary>
	///     Получить последний пройденный опрос по идентификатору пациента.
	/// </summary>
	/// <returns>Опрос.</returns>
	[HttpGet("last")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<SurveyModel> GetLastByPatientId([FromQuery] SurveyGetLastByPatientId.Request request,
		CancellationToken token) => await _mediator.Send(request, token);

	/// <summary>
	///     Получить признак необходимости пройти опрос по идентификатору пациента.
	/// </summary>
	/// <returns>Опрос.</returns>
	[HttpGet("necessity")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<bool> NecessityByPatientId([FromQuery] SurveyNecessityByPatientId.Request request,
		CancellationToken token) => await _mediator.Send(request, token);

	/// <summary>
	///     Получить опрос по идентификатору.
	/// </summary>
	/// <returns>Опрос.</returns>
	[HttpGet("{id:long}")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<SurveyModel> GetById([FromRoute] long id, CancellationToken token)
		=> await _mediator.Send(new SurveyGetById.Request { SurveyId = id }, token);

	/// <summary>
	///     Заполнить опрос.
	/// </summary>
	/// <returns>Опрос.</returns>
	[Display(Order = 1)]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[HttpPost]
	[Produces("application/json")]
	public async Task<SurveyModel> Add(SurveyAdd.Request request, CancellationToken token)
		=> await _mediator.Send(request, token);

	/// <summary>
	///     Редактировать опрос.
	/// </summary>
	/// <returns>Опрос.</returns>
	[Display(Order = 1)]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[HttpPut]
	[Produces("application/json")]
	public async Task<SurveyModel> Edit(SurveyEdit.Request request, CancellationToken token)
		=> await _mediator.Send(request, token);

	/// <summary>
	///     Удалить опрос.
	/// </summary>
	/// <returns>Результат.</returns>
	[HttpDelete("{id:long}")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public async Task<SurveyModel> Delete([FromRoute] long id, CancellationToken token)
		=> await _mediator.Send(new SurveyDelete.Request { SurveyId = id }, token);
}