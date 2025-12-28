using Geoapify.SDK.Geocoding.Inputs;
using Geoapify.SDK.Geocoding.Outputs;
using Geoapify.SDK.Geocoding.Response;

namespace Geoapify.SDK.Geocoding;

internal class GeocodingModule : IGeocodingModule
{
	private readonly string _apiKey;
	private readonly IHttpClientFactoryWrapper _httpClientFactory;
	private readonly JsonSerializerOptions _serializerOptions;
	private readonly TimeProvider _timeProvider;

	public GeocodingModule(IHttpClientFactoryWrapper httpClientFactory, JsonSerializerOptions serializerOptions, TimeProvider timeProvider, string apiKey)
	{
		_httpClientFactory = httpClientFactory;
		_serializerOptions = serializerOptions;
		_timeProvider = timeProvider;
		_apiKey = apiKey;
	}

	public async Task<IEnumerable<Address>> SearchAsync(string text, GeocodingSearchArguments? arguments = null, CancellationToken cancellationToken = default)
	{
		var queryStringBuilder = CreateQueryStringBuilder()
			.With("text", text);
		if (arguments is not null)
		{
			queryStringBuilder.With(arguments);
		}

		return await ExecuteSearchAsync(queryStringBuilder, cancellationToken);
	}

	public async Task<IEnumerable<Address>> SearchAsync(GeocodingStructuredSearch model, GeocodingSearchArguments? arguments = null, CancellationToken cancellationToken = default)
	{
		if (!((IQueryStringArgument)model).HasValues)
		{
			throw new ArgumentException("No arguments given for search", nameof(model));
		}

		var queryStringBuilder = CreateQueryStringBuilder()
			.With(model);
		if (arguments is not null)
		{
			queryStringBuilder.With(arguments);
		}

		return await ExecuteSearchAsync(queryStringBuilder, cancellationToken);
	}

	private QueryStringBuilder CreateQueryStringBuilder()
	{
		return new QueryStringBuilder()
			.With("apiKey", _apiKey)
			.With("format", "json");
	}

	private async Task<IEnumerable<Address>> ExecuteSearchAsync(QueryStringBuilder queryStringBuilder, CancellationToken cancellationToken)
	{
		var url = $"geocode/search?{queryStringBuilder.Build()}";

		var client = _httpClientFactory.CreateClient();
		var response = await client.GetAsync(url, cancellationToken);

		response.EnsureSuccessStatusCode();

#if DEBUG
		var json = await response.Content.ReadAsStringAsync(cancellationToken);
		var result = JsonSerializer.Deserialize<GeocodingJsonResponse>(json, _serializerOptions) ?? throw new InvalidOperationException("Could not deserialize GeocodingJsonResponse");
#else
		await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
		var result = await JsonSerializer.DeserializeAsync<GeocodingJsonResponse>(stream, _serializerOptions, cancellationToken) ?? throw new InvalidOperationException("Could not deserialize GeocodingJsonResponse");
#endif

		var utcNow = _timeProvider.GetUtcNow();

		return result.Results.Select(geocodingJson => Address.Create(geocodingJson, utcNow));
	}
}

/// <summary>
///     Forward Geocoding SDK used to search out addresses based on text.
///     <seealso href="https://apidocs.geoapify.com/docs/geocoding/forward-geocoding/" />
/// </summary>
public interface IGeocodingModule
{
	/// <summary>
	///     Search for one or more addresses via free-form text.
	///     Finds up to 5 results by default.
	/// </summary>
	/// <param name="text">Text to search for</param>
	/// <param name="arguments">Optional: Further filtration arguments, including number of results to find.</param>
	/// <param name="cancellationToken">CancellationToken</param>
	/// <returns>List of addresses found</returns>
	Task<IEnumerable<Address>> SearchAsync(string text, GeocodingSearchArguments? arguments = null, CancellationToken cancellationToken = default);

	/// <summary>
	///     Search for one or more addresses via a structured data model.
	///     Finds up to 5 results by default.
	/// </summary>
	/// <param name="model">Structured model used for searching</param>
	/// <param name="arguments">Optional: Further filtration arguments, including number of results to find.</param>
	/// <param name="cancellationToken">CancellationToken</param>
	/// <returns>List of addresses found</returns>
	Task<IEnumerable<Address>> SearchAsync(GeocodingStructuredSearch model, GeocodingSearchArguments? arguments = null, CancellationToken cancellationToken = default);
}