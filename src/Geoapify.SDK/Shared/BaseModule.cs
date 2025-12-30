namespace Geoapify.SDK.Shared;

internal abstract class BaseModule
{
	private readonly string _apiKey;
	private readonly string _baseUrl;
	private readonly IHttpClientFactoryWrapper _httpClientFactory;
	private readonly JsonSerializerOptions _serializerOptions;

	protected BaseModule(IHttpClientFactoryWrapper httpClientFactory, JsonSerializerOptions serializerOptions, string apiKey, string baseUrl)
	{
		_httpClientFactory = httpClientFactory;
		_serializerOptions = serializerOptions;
		_apiKey = apiKey;
		_baseUrl = baseUrl;
	}

	protected QueryStringBuilder CreateQueryStringBuilder()
	{
		return new QueryStringBuilder()
			.With("apiKey", _apiKey)
			.With("format", "json");
	}

	protected async Task<TResponse> ExecuteQueryAsync<TResponse>(QueryStringBuilder queryStringBuilder, CancellationToken cancellationToken)
	{
		var url = $"{_baseUrl}?{queryStringBuilder.Build()}";

		var client = _httpClientFactory.CreateClient();
		var response = await client.GetAsync(url, cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			var body = await response.Content.ReadAsStringAsync(cancellationToken);
			throw new HttpRequestException(body, null, response.StatusCode);
		}

#if DEBUG
		var json = await response.Content.ReadAsStringAsync(cancellationToken);
		return JsonSerializer.Deserialize<TResponse>(json, _serializerOptions) ?? throw new InvalidOperationException($"Could not deserialize {typeof(TResponse).Name}");
#else
		await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
		return await JsonSerializer.DeserializeAsync<TResponse>(stream, _serializerOptions, cancellationToken) ?? throw new InvalidOperationException($"Could not deserialize {typeof(TResponse).Name}");
#endif
	}
}