using System.ComponentModel.DataAnnotations;
using Hfd.BizLayer.Features;
using Hfd.Common.ApiClient.Model;
using Hfd.Contract.Dto;
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
    public PatientController(IMediator mediator) => this._mediator = mediator;

    /// <summary>
    ///     Получить список всех пациентов.
    /// </summary>
    /// <returns>Список пациентов.</returns>
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<PageList<PatientModel>> GetAll([FromQuery] PatientGetAll.Request request, CancellationToken token)
        => await this._mediator.Send(request, token);

    /// <summary>
    ///     Получить пациента по идентификатору.
    /// </summary>
    /// <returns>Пациент.</returns>
    [HttpGet("{id:long}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<PatientModel> GetById([FromRoute] long id, CancellationToken token)
        => await this._mediator.Send(new PatientGetById.Request { PatientId = id }, token);

    /// <summary>
    ///     Получить контакты сотрудников привязанных к пациенту.
    /// </summary>
    /// <returns>Пациент.</returns>
    [HttpGet("{id:long}/staff_contact")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<PatientGetStaffContactsById.StaffContact> GetStaffContactsById([FromRoute] long id, CancellationToken token)
        => await this._mediator.Send(new PatientGetStaffContactsById.Request { PatientId = id }, token);

    /// <summary>
    ///     Получить пациента по идентификатору пользователя.
    /// </summary>
    /// <returns>Пациент.</returns>
    [HttpGet("user/{userId:long}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<PatientModel> GetByUserId([FromRoute] long userId, CancellationToken token)
        => await this._mediator.Send(new PatientGetByUserId.Request { UserId = userId }, token);

    /// <summary>
    ///     Добавить пациента.
    /// </summary>
    /// <returns>Пациент.</returns>
    [Display(Order = 1)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpPost]
    [Produces("application/json")]
    public async Task<PatientModel> Add(PatientAdd.Request request, CancellationToken token)
        => await this._mediator.Send(request, token);

    /// <summary>
    ///     Редактировать пациента.
    /// </summary>
    /// <returns>Пациент.</returns>
    [Display(Order = 1)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpPut]
    [Produces("application/json")]
    public async Task<PatientModel> Edit(PatientEdit.Request request, CancellationToken token)
        => await this._mediator.Send(request, token);

    /// <summary>
    ///     Удалить пациента.
    /// </summary>
    /// <returns>Результат.</returns>
    [HttpDelete("{id:long}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<PatientModel> Delete([FromRoute] long id, CancellationToken token)
        => await this._mediator.Send(new PatientDelete.Request { PatientId = id }, token);
}
