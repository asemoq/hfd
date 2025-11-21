using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Hfd.BizLayer.Features;
using Hfd.Common.Contract;
using Hfd.Common.Models;
using Hfd.Contract.Model;
using Hfd.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hfd.Backend.Controllers;

/// <summary>
///     Управление участками.
/// </summary>
[ApiController]
[Route("plot")]
[Authorize(Roles = "Administrator")]
[Display(Order = 2, Name = "Участки")]
public class PlotController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    ///     Получить список всех участков.
    /// </summary>
    /// <returns>Список медсестер.</returns>
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<PageList<PlotModel>> GetAll([FromQuery] PlotGetAll.Request request, CancellationToken token)
        => mapper.Map<IPlotFull, PlotModel>(await mediator.Send(request, token));

    /// <summary>
    ///     Получить участок по идентификатору.
    /// </summary>
    /// <returns>Медсестра.</returns>
    [HttpGet("{id:long}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<PlotModel> GetById([FromRoute] long id, CancellationToken token)
        => mapper.Map<PlotModel>(await mediator.Send(new PlotGetById.Request { PlotId = id }, token));

    /// <summary>
    ///     Добавить участок.
    /// </summary>
    /// <returns>Медсестра.</returns>
    [Display(Order = 1)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpPost]
    [Produces("application/json")]
    public async Task<PlotModel> Add(PlotAdd.Request request, CancellationToken token)
        => mapper.Map<PlotModel>(await mediator.Send(request, token));

    /// <summary>
    ///     Редактировать участок.
    /// </summary>
    /// <returns>Медсестра.</returns>
    [Display(Order = 1)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpPut]
    [Produces("application/json")]
    public async Task<PlotModel> Edit(PlotEdit.Request request, CancellationToken token)
        => mapper.Map<PlotModel>(await mediator.Send(request, token));

    /// <summary>
    ///     Удалить участок.
    /// </summary>
    /// <returns>Результат.</returns>
    [HttpDelete("{id:long}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<PlotModel> Delete([FromRoute] long id, CancellationToken token)
        => mapper.Map<PlotModel>(await mediator.Send(new PlotDelete.Request { PlotId = id }, token));
}
