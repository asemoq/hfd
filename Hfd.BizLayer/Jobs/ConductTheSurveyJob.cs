using Hfd.BizLayer.Internals;
using Hfd.Common.Contract.Attributes;
using RestEase;

namespace Hfd.BizLayer.Jobs;

/// <summary>
/// Задача отправки уведомлений.
/// </summary>
[NamedService("ConductTheSurvey:Job")]
public sealed class ConductTheSurveyJob(IDbStorage storage, IAdminClientFactory factory, IProviderOptions options) : IExecutant
{
    /// <inheritdoc />
    public async Task Execute(IExecutionContext context)
    {
        if (options.TgUri == null) return;
        using var client = await factory.CreateClient(options.TgUri);
        var patients = await storage.GetService<IPatientService>().GetForReminder();
        foreach (var patient in patients)
        {
            if (!Guid.TryParse(patient.UserId, out var guid)) continue;

            try
            {
                await client.SendNotification(guid.ToString(), context.Aborted);
            }
            catch (ApiException e)
            {
                Console.WriteLine(e.Content);
                throw;
            }
        }
    }
}
