using Hfd.Common.Contract.Attributes;
using Microsoft.Extensions.Logging;

namespace Hfd.BizLayer.Jobs;

/// <summary>
///     Задача запроса проведения опроса.
/// </summary>
[NamedService("RequestSurvey:Job")]
public sealed class RequestSurveyJob : IExecutant
{
    public readonly IBizContext _context;
    private readonly ILogger<RequestSurveyJob> _log;

    public RequestSurveyJob(IBizContext context, ILogger<RequestSurveyJob> log)
    {
        (this._context, this._log) = (context, log);
    }

    /// <inheritdoc />
    public async Task Execute(IExecutionContext context)
    {
        var service = this._context.Services.GetRequiredService<IPatientService>();
        var surveyService = this._context.Services.GetRequiredService<ISurveyConductionService>();
        var patients = await service.GetForRequest();
        foreach (var patient in patients)
        {
            await surveyService.Request(patient);
        }
    }
}
