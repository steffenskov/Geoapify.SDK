using Geoapify.SDK.Geocoding.Inputs;
using Geoapify.SDK.Geocoding.Response;

namespace Geoapify.SDK.Geocoding;

internal class AutocompleteModule : BaseModule, IAutocompleteModule
{
	private readonly TimeProvider _timeProvider;

	public AutocompleteModule(IHttpClientFactoryWrapper httpClientFactory, JsonSerializerOptions serializerOptions, TimeProvider timeProvider, string apiKey) : base(httpClientFactory, serializerOptions, apiKey, "geocode/autocomplete")
	{
		_timeProvider = timeProvider;
	}

	public async Task<IEnumerable<Address>> AutocompleteAsync(string text, GeocodingAutocompleteArguments? arguments = null, CancellationToken cancellationToken = default)
	{
		var queryStringBuilder = CreateQueryStringBuilder()
			.With("text", text);
		arguments ??= new GeocodingAutocompleteArguments();
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
public interface IAutocompleteModule
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
	Task<IEnumerable<Address>> AutocompleteAsync(string text, GeocodingAutocompleteArguments? arguments = null, CancellationToken cancellationToken = default);
}