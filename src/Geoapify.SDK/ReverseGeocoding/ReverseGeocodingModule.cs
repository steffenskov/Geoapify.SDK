using System.Globalization;
using Geoapify.SDK.ReverseGeocoding.Inputs;
using Geoapify.SDK.ReverseGeocoding.Response;
using Geoapify.SDK.Shared;
using Geoapify.SDK.Shared.Outputs;

namespace Geoapify.SDK.ReverseGeocoding;

internal class ReverseGeocodingModule : BaseModule, IReverseGeocodingModule
{
	private readonly TimeProvider _timeProvider;

	public ReverseGeocodingModule(IHttpClientFactoryWrapper httpClientFactory, JsonSerializerOptions serializerOptions, TimeProvider timeProvider, string apiKey) : base(httpClientFactory, serializerOptions, apiKey, "geocode/reverse")
	{
		_timeProvider = timeProvider;
	}

	public async Task<IEnumerable<Address>> SearchAsync(double latitude, double longitude, ReverseGeocodingSearchArguments? arguments = null, CancellationToken cancellationToken = default)
	{
		var queryStringBuilder = CreateQueryStringBuilder()
			.With("lat", latitude.ToString(CultureInfo.InvariantCulture))
			.With("lon", longitude.ToString(CultureInfo.InvariantCulture));
		if (arguments is not null)
		{
			queryStringBuilder.With(arguments);
		}

		return await ExecuteSearchAsync(queryStringBuilder, cancellationToken);
	}


	private async Task<IEnumerable<Address>> ExecuteSearchAsync(QueryStringBuilder queryStringBuilder, CancellationToken cancellationToken = default)
	{
		var result = await ExecuteQueryAsync<ReverseGeocodingJsonResponse>(queryStringBuilder, cancellationToken);

		var utcNow = _timeProvider.GetUtcNow();

		return result.Results.Select(geocodingJson => Address.Create(geocodingJson, utcNow));
	}
}

public interface IReverseGeocodingModule
{
	/// <summary>
	///     Search out a single address based on its lat/lon coordinates.
	///     Finds 1 result by default.
	/// </summary>
	/// <param name="latitude">Latitude to search for</param>
	/// <param name="longitude">Longitude to search for</param>
	/// <param name="arguments">Optional: Further filtration arguments, including number of results to find (default: 1).</param>
	/// <param name="cancellationToken">CancellationToken</param>
	/// <returns>List of addresses found</returns>
	Task<IEnumerable<Address>> SearchAsync(double latitude, double longitude, ReverseGeocodingSearchArguments? arguments = null, CancellationToken cancellationToken = default);

	/// <summary>
	///     Search out a single address based on its lat/lon coordinates.
	///     Finds 1 result by default.
	/// </summary>
	/// <param name="coordinate">Coordinate to search for</param>
	/// <param name="arguments">Optional: Further filtration arguments, including number of results to find (default: 1).</param>
	/// <param name="cancellationToken">CancellationToken</param>
	/// <returns>List of addresses found</returns>
	Task<IEnumerable<Address>> SearchAsync(Coordinate coordinate, ReverseGeocodingSearchArguments? arguments = null, CancellationToken cancellationToken = default)
	{
		return SearchAsync(coordinate.Latitude, coordinate.Longitude, arguments, cancellationToken);
	}
}