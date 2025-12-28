using Geoapify.SDK.Configuration;
using Geoapify.SDK.Geocoding;

namespace Geoapify.SDK.Client;

public class GeoapifyClient : IGeoapifyClient
{
	public GeoapifyClient(IHttpClientFactory httpClientFactory, TimeProvider timeProvider, string apiKey)
	{
		var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
		{
			Converters = { new JsonStringEnumConverter() }
		};
		Geocoding = new GeocodingModule(new HttpClientFactoryWrapper(httpClientFactory, nameof(GeoapifyClient)), serializerOptions, timeProvider, apiKey);
	}

	public GeoapifyClient(IHttpClientFactory httpClientFactory, IOptions<GeoapifyConfiguration> options, TimeProvider timeProvider) : this(httpClientFactory, timeProvider, options.Value.ApiKey)
	{
	}

	public IGeocodingModule Geocoding { get; }
}

public interface IGeoapifyClient
{
	IGeocodingModule Geocoding { get; }
}