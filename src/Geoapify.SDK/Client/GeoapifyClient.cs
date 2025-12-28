using Geoapify.SDK.Configuration;
using Geoapify.SDK.Geocoding;

namespace Geoapify.SDK.Client;

public class GeoapifyClient : IGeoapifyClient
{
	public GeoapifyClient(IHttpClientFactory httpClientFactory, string apiKey)
	{
		var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
		{
			Converters = { new JsonStringEnumConverter() }
		};
		Geocoding = new GeocodingModule(new HttpClientFactoryWrapper(httpClientFactory, nameof(GeoapifyClient)), serializerOptions, apiKey);
	}

	public GeoapifyClient(IHttpClientFactory httpClientFactory, IOptions<GeoapifyConfiguration> options) : this(httpClientFactory, options.Value.ApiKey)
	{
	}

	public IGeocodingModule Geocoding { get; }
}

public interface IGeoapifyClient
{
	IGeocodingModule Geocoding { get; }
}