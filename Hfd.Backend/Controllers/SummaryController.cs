using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Hfd.BizLayer.Features;
using Hfd.Common.Contract.Defaults;
using Hfd.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hfd.Backend.Controllers;

/// <summary>
///     Управление сводкой.
/// </summary>
[ApiController]
[Route("summary")]
[Display(Order = 4, Name = "Сводка")]
[Authorize(AuthenticationSchemes = WebAuthenticationDefaults.AuthenticationScheme)]
public class SummaryController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    ///     Получить сводку за месяц.
    /// </summary>
    /// <returns>Сводка.</returns>
    [HttpGet("month")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<List<SurveyModel>> Get([FromQuery] SummaryGet.Request request, CancellationToken token)
        => await mediator.Send(request, token);

    /// <summary>
    ///     Получить сводку за период по индикатору.
    /// </summary>
    /// <returns>Сводка.</returns>
    [HttpGet("period")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<List<SurveyChartModel>?> GetByIndicator([FromQuery] SummaryGetByIndicator.Request request, CancellationToken token)
        => await mediator.Send(request, token);
}
