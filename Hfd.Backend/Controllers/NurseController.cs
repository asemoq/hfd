using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Hfd.BizLayer.Features;
using Hfd.Common.Contract;
using Hfd.Common.Contract.Model;
using Hfd.Common.Models;
using Hfd.Contract.Model;
using Hfd.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hfd.Backend.Controllers;

/// <summary>
///     Управление медсестрами.
/// </summary>
[ApiController]
[Route("nurse")]
[Authorize(Roles = "Administrator")]
[Display(Order = 2, Name = "Медсестры")]
public class NurseController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    ///     Получить список всех медсестер.
    /// </summary>
    /// <returns>Список медсестер.</returns>
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<PageList<NurseModel>> GetAll([FromQuery] NurseGetAll.Request request, CancellationToken token)
        => mapper.Map<INurseFull, NurseModel>(await mediator.Send(request, token));

    /// <summary>
    ///     Получить медсестру по идентификатору.
    /// </summary>
    /// <returns>Медсестра.</returns>
    [HttpGet("{id:long}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<NurseModel> GetById([FromRoute] long id, CancellationToken token)
        => mapper.Map<INurseFull, NurseModel>(await mediator.Send(new NurseGetById.Request { NurseId = id }, token));

    /// <summary>
    ///     Получить медсестру по идентификатору пользователя.
    /// </summary>
    /// <returns>Медсестра.</returns>
    [HttpGet("user/{userId}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<NurseModel> GetByUserId([FromRoute] string userId, CancellationToken token)
        => mapper.Map<INurseFull, NurseModel>(await mediator.Send(new NurseGetByUserId.Request { UserId = userId }, token));

    /// <summary>
    ///     Добавить медсестру.
    /// </summary>
    /// <returns>Медсестра.</returns>
    [Display(Order = 1)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpPost]
    [Produces("application/json")]
    public async Task<NurseModel> Add(NurseAdd.Request request, CancellationToken token)
        => mapper.Map<INurseFull, NurseModel>(await mediator.Send(request, token));

    /// <summary>
    ///     Редактировать медсестру.
    /// </summary>
    /// <returns>Медсестра.</returns>
    [Display(Order = 1)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpPut]
    [Produces("application/json")]
    public async Task<NurseModel> Edit(NurseEdit.Request request, CancellationToken token)
        => mapper.Map<INurseFull, NurseModel>(await mediator.Send(request, token));

    /// <summary>
    ///     Удалить медсестру.
    /// </summary>
    /// <returns>Результат.</returns>
    [HttpDelete("{id:long}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<NurseModel> Delete([FromRoute] long id, CancellationToken token)
        => mapper.Map<INurseFull, NurseModel>(await mediator.Send(new NurseDelete.Request { NurseId = id }, token));
}
