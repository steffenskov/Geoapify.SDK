using Geoapify.SDK.Geocoding.Inputs;
using Geoapify.SDK.Geocoding.Response;

namespace Geoapify.SDK.Geocoding;

internal class GeocodingModule : BaseModule, IGeocodingModule
{
	private readonly TimeProvider _timeProvider;

	public GeocodingModule(IHttpClientFactoryWrapper httpClientFactory, JsonSerializerOptions serializerOptions, TimeProvider timeProvider, string apiKey) : base(httpClientFactory, serializerOptions, apiKey, "geocode/search")
	{
		_timeProvider = timeProvider;
	}

	public async Task<IEnumerable<Address>> SearchAsync(string text, GeocodingSearchArguments? arguments = null, CancellationToken cancellationToken = default)
	{
		var queryStringBuilder = CreateQueryStringBuilder()
			.With("text", text);
		arguments ??= new GeocodingSearchArguments();
		queryStringBuilder.With(arguments);

		return await ExecuteSearchAsync(queryStringBuilder, arguments.Language, cancellationToken);
	}

	public async Task<IEnumerable<Address>> SearchAsync(GeocodingStructuredSearch model, GeocodingSearchArguments? arguments = null, CancellationToken cancellationToken = default)
	{
		if (!((IQueryStringArgument)model).HasValues)
		{
			throw new ArgumentException("No arguments given for search", nameof(model));
		}

		arguments ??= new GeocodingSearchArguments();

		var queryStringBuilder = CreateQueryStringBuilder()
			.With(model);
		queryStringBuilder.With(arguments);

		return await ExecuteSearchAsync(queryStringBuilder, arguments.Language, cancellationToken);
	}

	private async Task<IEnumerable<Address>> ExecuteSearchAsync(QueryStringBuilder queryStringBuilder, Language language, CancellationToken cancellationToken = default)
	{
		var result = await ExecuteQueryAsync<GeocodingJsonResponse>(queryStringBuilder, cancellationToken);

		var utcNow = _timeProvider.GetUtcNow();

		return result.Results.Select(geocodingJson => Address.Create(geocodingJson, language, utcNow));
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
	/// <param name="arguments">
	///     Optional: Further filtration arguments, including number of results to find (default: default properties of
	///     GeocodingSearchArguments).
	/// </param>
	/// <param name="cancellationToken">CancellationToken</param>
	/// <returns>List of addresses found</returns>
	Task<IEnumerable<Address>> SearchAsync(string text, GeocodingSearchArguments? arguments = null, CancellationToken cancellationToken = default);

	/// <summary>
	///     Search for one or more addresses via a structured data model.
	///     Finds up to 5 results by default.
	/// </summary>
	/// <param name="model">Structured model used for searching</param>
	/// <param name="arguments">
	///     Optional: Further filtration arguments, including number of results to find (default: default properties of
	///     GeocodingSearchArguments).
	/// </param>
	/// <param name="cancellationToken">CancellationToken</param>
	/// <returns>List of addresses found</returns>
	Task<IEnumerable<Address>> SearchAsync(GeocodingStructuredSearch model, GeocodingSearchArguments? arguments = null, CancellationToken cancellationToken = default);
}