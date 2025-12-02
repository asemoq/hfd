using AutoMapper;
using Hfd.Common.Contract.Attributes;
using Hfd.Provider.Grpc.Model;
using Hfd.Provider.Grpc.Services;
using RestEase;

namespace Hfd.BizLayer.Jobs;

/// <summary>
/// Задача отправки уведомлений.
/// </summary>
[NamedService("ConductTheSurvey:Job")]
public sealed class ConductTheSurveyJob(IDbStorage storage, IProviderService service, IUserService userService, IMapper mapper) : IExecutant
{
    /// <inheritdoc />
    public async Task Execute(IExecutionContext context)
    {
        var patients = await storage.GetService<IPatientService>().GetForReminder();
        foreach (var patient in patients)
        {
            if (!Guid.TryParse(patient.UserId, out var guid)) continue;

            try
            {
                var user = await userService.Find(guid.ToString());
                if (user != null) await service.ConductTheSurvey(mapper.Map<UserData>(user));
            }
            catch (ApiException e)
            {
                Console.WriteLine(e.Content);
                throw;
            }
        }
    }
}
