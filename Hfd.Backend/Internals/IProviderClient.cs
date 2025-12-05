using Hfd.Common.Models;
using Hfd.Common.Services;
using RestEase;

namespace Hfd.Backend.Internals;

public interface IProviderClient : IAuthorizedClient
{
    /// <summary>
    /// Изменить язык текущего пользователя.
    /// </summary>
    [Post("/update-language")]
    Task<UserFull> UpdateLanguage(CancellationToken token = default);
}
