using Hfd.Common.Services;
using Hfd.Provider.Services;

namespace Hfd.BizLayer.Internals;

public interface IAdminClientFactory
{
    /// <summary>
    /// Создаёт клиент для сервисных операций.
    /// </summary>
    /// <returns>Экземпляр клиента.</returns>
    Task<IProviderClient> CreateClient(Uri uri, int timeout = 0);
}
