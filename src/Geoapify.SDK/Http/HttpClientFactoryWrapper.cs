namespace Geoapify.SDK.Http;

internal interface IHttpClientFactoryWrapper
{
	HttpClient CreateClient();
}

internal class HttpClientFactoryWrapper : IHttpClientFactoryWrapper
{
	private readonly string _clientName;
	private readonly IHttpClientFactory _httpClientFactory;

	public HttpClientFactoryWrapper(IHttpClientFactory httpClientFactory, string clientName)
	{
		_httpClientFactory = httpClientFactory;
		_clientName = clientName;
	}

	public HttpClient CreateClient()
	{
		return _httpClientFactory.CreateClient(_clientName);
	}
}