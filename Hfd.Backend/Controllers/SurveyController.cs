using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Hfd.BizLayer.Features;
using Hfd.Common.Contract;
using Hfd.Common.Contract.Defaults;
using Hfd.Common.Models;
using Hfd.Contract.Enums;
using Hfd.Contract.Model;
using Hfd.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hfd.Backend.Controllers;

/// <summary>
///     Управление опросами.
/// </summary>
[ApiController]
[Route("survey")]
[Display(Order = 4, Name = "Опросы")]
[Authorize(AuthenticationSchemes = WebAuthenticationDefaults.AuthenticationScheme)]
public class SurveyController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    ///     Получить список всех опросов.
    /// </summary>
    /// <returns>Список опросов.</returns>
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<PageList<SurveyModel>> GetAll([FromQuery] SurveyGetAll.Request request, CancellationToken token)
        => mapper.Map<ISurveyFull, SurveyModel>(await mediator.Send(request, token));

    /// <summary>
    ///     Получить последний пройденный опрос по идентификатору пациента.
    /// </summary>
    /// <returns>Опрос.</returns>
    [HttpGet("last")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<SurveyModel> GetLastByPatientId([FromQuery] SurveyGetLastByPatientId.Request request, CancellationToken token)
        => mapper.Map<ISurveyFull, SurveyModel>(await mediator.Send(request, token));

    /// <summary>
    ///     Получить признак необходимости пройти опрос по идентификатору пациента.
    /// </summary>
    /// <returns>Опрос.</returns>
    [HttpGet("necessity")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<bool> NecessityByPatientId([FromQuery] SurveyNecessityByPatientId.Request request, CancellationToken token)
        => await mediator.Send(request, token);

    /// <summary>
    ///     Получить опрос по идентификатору.
    /// </summary>
    /// <returns>Опрос.</returns>
    [HttpGet("{id:long}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<SurveyModel> GetById([FromRoute] long id, CancellationToken token)
        => mapper.Map<SurveyModel>(await mediator.Send(new SurveyGetById.Request { SurveyId = id }, token));

    /// <summary>
    ///     Заполнить опрос.
    /// </summary>
    /// <returns>Опрос.</returns>
    [Display(Order = 1)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpPost]
    [Produces("application/json")]
    public async Task<SurveyModel> Add(SurveyAdd.Request request, CancellationToken token)
        => mapper.Map<SurveyModel>(await mediator.Send(request, token));

    /// <summary>
    ///     Редактировать опрос.
    /// </summary>
    /// <returns>Опрос.</returns>
    [Display(Order = 1)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpPut]
    [Produces("application/json")]
    public async Task<SurveyModel> Edit(SurveyEdit.Request request, CancellationToken token)
        => mapper.Map<SurveyModel>(await mediator.Send(request, token));

    /// <summary>
    ///     Удалить опрос.
    /// </summary>
    /// <returns>Результат.</returns>
    [HttpDelete("{id:long}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<SurveyModel> Delete([FromRoute] long id, CancellationToken token)
        => mapper.Map<SurveyModel>(await mediator.Send(new SurveyDelete.Request { SurveyId = id }, token));
}
