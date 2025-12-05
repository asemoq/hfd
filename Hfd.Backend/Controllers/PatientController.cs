using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Hfd.BizLayer.Features;
using Hfd.Common.Contract;
using Hfd.Common.Contract.Defaults;
using Hfd.Common.Models;
using Hfd.Contract.Model;
using Hfd.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hfd.Backend.Controllers;

/// <summary>
///     Управление пациентами.
/// </summary>
[ApiController]
[Route("patient")]
[Display(Order = 3, Name = "Пациенты")]
[Authorize(Roles = "Administrator,Nurse", AuthenticationSchemes = WebAuthenticationDefaults.AuthenticationScheme)]
public class PatientController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    ///     Получить список всех пациентов.
    /// </summary>
    /// <returns>Список пациентов.</returns>
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<PageList<PatientModel>> GetAll([FromQuery] PatientGetAll.Request request, CancellationToken token)
        => mapper.Map<IPatientFull, PatientModel>(await mediator.Send(request, token));

    /// <summary>
    ///     Получить пациента по идентификатору.
    /// </summary>
    /// <returns>Пациент.</returns>
    [HttpGet("{id:long}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<PatientModel> GetById([FromRoute] long id, CancellationToken token)
        => mapper.Map<PatientModel>(await mediator.Send(new PatientGetById.Request { PatientId = id }, token));

    /// <summary>
    ///     Получить контакты сотрудников привязанных к пациенту.
    /// </summary>
    /// <returns>Пациент.</returns>
    [HttpGet("{id:long}/staff-contact")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<StaffContact> GetStaffContactsById([FromRoute] long id, CancellationToken token)
        => mapper.Map<StaffContact>(await mediator.Send(new PatientGetById.Request { PatientId = id }, token));

    /// <summary>
    ///     Добавить пациента.
    /// </summary>
    /// <returns>Пациент.</returns>
    [Display(Order = 1)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpPost]
    [Produces("application/json")]
    public async Task<PatientModel> Add(PatientAdd.Request request, CancellationToken token)
        => mapper.Map<PatientModel>(await mediator.Send(request, token));

    /// <summary>
    ///     Редактировать пациента.
    /// </summary>
    /// <returns>Пациент.</returns>
    [Display(Order = 1)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpPut]
    [Produces("application/json")]
    public async Task<PatientModel> Edit(PatientEdit.Request request, CancellationToken token)
        => mapper.Map<PatientModel>(await mediator.Send(request, token));

    /// <summary>
    ///     Удалить пациента.
    /// </summary>
    /// <returns>Результат.</returns>
    [HttpDelete("{id:long}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<PatientModel> Delete([FromRoute] long id, CancellationToken token)
        => mapper.Map<PatientModel>(await mediator.Send(new PatientDelete.Request { PatientId = id }, token));

    /// <summary>
    ///     Получить список пациентов для напоминания о прохождении опроса.
    /// </summary>
    /// <returns>Список пациентов.</returns>
    [HttpGet("remind")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IEnumerable<PatientModel>> GetForReminder(CancellationToken token)
        => mapper.Map<IPatientFull, PatientModel>(await mediator.Send(new PatientGetForReminder.Request(), token));
}
