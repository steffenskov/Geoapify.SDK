using Geoapify.SDK.Configuration;
using Geoapify.SDK.Geocoding;
using Geoapify.SDK.Http;
using Microsoft.Extensions.Options;

namespace Geoapify.SDK.Client;

public class GeoapifyClient : IGeoapifyClient
{
	public GeoapifyClient(IHttpClientFactory httpClientFactory, string apiKey)
	{
		Geocoding = new GeocodingModule(new HttpClientFactoryWrapper(httpClientFactory, nameof(GeoapifyClient)), apiKey);
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