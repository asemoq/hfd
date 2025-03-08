using Hfd.Base.Contract.Services;
using RestEase;

namespace Hfd.Base.BizLayer.Services
{
	/// <summary>
	/// Клиент провайдера
	/// </summary>
	public interface IProviderClient : IClient
	{
		[Post("/login")]
		Task<object> Login([Body] object data);
	}
}