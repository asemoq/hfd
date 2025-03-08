using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestEase;

namespace Hfd.Base.Contract.Services;

/// <summary>
/// Фабрика построения клиента.
/// </summary>
public static class ClientFactory
{
	/// <summary>
	/// Имя клиента для пользовательских операций.
	/// </summary>
	public const string UserClientName = "userClient";

	/// <summary>
	/// Имя клиента для серверных операций.
	/// </summary>
	public const string ServiceClientName = "serviceClient";

	/// <inheritdoc cref="JsonSerializerSettings"/>
	public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
	{
		NullValueHandling = NullValueHandling.Ignore,
		ContractResolver = new CamelCasePropertyNamesContractResolver(),
	};

	/// <summary>
	/// Инициализирует новый экземпляр клиента.
	/// </summary>
	/// <param name="client">Сконфигурированный httpClient.</param>
	/// <returns>Новый экземпляр клиента.</returns>
	public static T Create<T>(HttpClient client) where T : IClient
		=> new RestClient(client) { JsonSerializerSettings = JsonSerializerSettings }.For<T>();

	/// <summary>
	/// Инициализирует новый экземпляр клиента.
	/// </summary>
	/// <param name="uri">Базовый адрес API сервера.</param>
	/// <returns>Новый экземпляр клиента.</returns>
	public static T Create<T>(Uri uri) where T : IClient
		=> new RestClient(uri) { JsonSerializerSettings = JsonSerializerSettings }.For<T>();

	/// <summary>
	/// Инициализирует новый экземпляр клиента.
	/// </summary>
	/// <param name="uri">Базовый адрес API сервера.</param>
	/// <param name="requestModifier">Delegate called on every request.</param>
	/// <returns>Новый экземпляр клиента.</returns>
	public static T Create<T>(Uri uri, RequestModifier requestModifier) where T : IClient
		=> new RestClient(uri, requestModifier) { JsonSerializerSettings = JsonSerializerSettings }.For<T>();
}