using System.Linq;
using System.Net.Http;
using System.Text;
using Hfd.Base.Contract.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Hfd.Base.BizLayer.Services;

/// <summary>
/// Фабрика клиентов подсистем.
/// </summary>
public interface IClientFactory<T> where T : IClient
{
	/// <summary>
	/// Создаёт клиент для пользовательских операций.
	/// </summary>
	/// <returns>Экземпляр клиента.</returns>
	T CreateUserClient(Uri uri, int timeout = 0);
}

/// <inheritdoc cref="IClientFactory{T}"/>
public class ClientFactory<T> : IClientFactory<T> where T : IClient
{
	/// <summary>
	/// <see cref="IHttpClientFactory"/>.
	/// </summary>
	private readonly IServiceProvider _services;

	/// <summary>
	/// Инициализирует новый экземпляр класса <see cref="ClientFactory"/>.
	/// </summary>
	/// <param name="services"><see cref="IHttpClientFactory"/>.</param>
	public ClientFactory(IServiceProvider services)
		=> _services = services;

	/// <inheritdoc />
	public T CreateUserClient(Uri uri, int timeout)
	{
		var client = _services.GetRequiredService<IHttpClientFactory>()
			.CreateClient(ClientFactory.UserClientName);
		client.Timeout = timeout <= 0 ? Timeout.InfiniteTimeSpan : TimeSpan.FromSeconds(timeout);
		client.BaseAddress = uri;

		var typedClient = ClientFactory.Create<T>(client);
		var httpRq = _services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Request;
		if (httpRq != null)
		{
			var auth = httpRq.Headers["Authorization"].SingleOrDefault(a => a != null && a.StartsWith("Bearer"));
			typedClient.Authorization = auth ?? string.Empty;
		}

		var userFull = _services.GetRequiredService<IBizContext>().UserFull;
		if (userFull != null)
		{
			var jsonUser = JsonConvert.SerializeObject(userFull, ClientFactory.JsonSerializerSettings);
			typedClient.UserFull = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonUser));
		}

		return typedClient;
	}
}