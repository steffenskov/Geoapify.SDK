using Geoapify.SDK.Configuration;
using Geoapify.SDK.Geocoding;
using Geoapify.SDK.ReverseGeocoding;

namespace Geoapify.SDK.Client;

public class GeoapifyClient : IGeoapifyClient
{
	public GeoapifyClient(IHttpClientFactory httpClientFactory, TimeProvider timeProvider, string apiKey)
	{
		var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
		{
			Converters = { new JsonStringEnumConverter() }
		};
		var httpClientFactoryWrapper = new HttpClientFactoryWrapper(httpClientFactory, nameof(GeoapifyClient));
		Geocoding = new GeocodingModule(httpClientFactoryWrapper, serializerOptions, timeProvider, apiKey);
		ReverseGeocoding = new ReverseGeocodingModule(httpClientFactoryWrapper, serializerOptions, timeProvider, apiKey);
	}

	public GeoapifyClient(IHttpClientFactory httpClientFactory, IOptions<GeoapifyConfiguration> options, TimeProvider timeProvider) : this(httpClientFactory, timeProvider, options.Value.ApiKey)
	{
	}

	public IGeocodingModule Geocoding { get; }
	public IReverseGeocodingModule ReverseGeocoding { get; }
}

public interface IGeoapifyClient
{
	IGeocodingModule Geocoding { get; }
	IReverseGeocodingModule ReverseGeocoding { get; }
}