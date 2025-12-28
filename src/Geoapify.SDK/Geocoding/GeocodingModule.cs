using System.Text.Json;
using System.Web;
using Geoapify.SDK.Http;

namespace Geoapify.SDK.Geocoding;

internal class GeocodingModule : IGeocodingModule
{
	private readonly string _apiKey;
	private readonly IHttpClientFactoryWrapper _httpClientFactory;

	public GeocodingModule(IHttpClientFactoryWrapper httpClientFactory, string apiKey)
	{
		_httpClientFactory = httpClientFactory;
		_apiKey = apiKey;
	}

	public async Task<IEnumerable<Address>> SearchAsync(string text, CancellationToken cancellationToken = default)
	{
		var client = _httpClientFactory.CreateClient();
		var encodedQuery = HttpUtility.UrlEncode(text);

		var url = $"geocode/search?text={encodedQuery}&apiKey={_apiKey}&format=json";

		var response = await client.GetAsync(url, cancellationToken);

		response.EnsureSuccessStatusCode();

		await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
		var result = await JsonSerializer.DeserializeAsync<GeocodingResponse>(stream, new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellationToken) ?? throw new InvalidOperationException("Could not deserialize GeocodingResponse");

		return result.Results;
	}
}

public interface IGeocodingModule
{
	Task<IEnumerable<Address>> SearchAsync(string text, CancellationToken cancellationToken = default);
}