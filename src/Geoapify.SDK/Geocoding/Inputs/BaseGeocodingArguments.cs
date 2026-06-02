using Geoapify.SDK.Geocoding.Inputs.Biases;
using Geoapify.SDK.Geocoding.Inputs.Filters;
using Geoapify.SDK.Geocoding.Inputs.SearchAreas;

namespace Geoapify.SDK.Geocoding.Inputs;

public abstract class BaseGeocodingArguments : IQueryStringArgument
{
	/// <summary>
	///     Type of locations to find, defaults to null which includes all types in the search result.
	/// </summary>
	public LocationTypes? Type { get; set; }

	/// <summary>
	///     Language to return data in, defaults to English.
	/// </summary>
	public Language Language { get; set; } = Language.English;

	/// <summary>
	///     How many results to return, defaults to 5.
	/// </summary>
	public uint Limit { get; set; } = 5;

	/// <summary>
	///     Filters to apply to search, defaults to all properties set to null, which means no filtering.
	///     When using multiple filters, they're AND'ed together.
	///     <seealso cref="https://apidocs.geoapify.com/docs/geocoding/forward-geocoding/" />
	/// </summary>
	public Filter Filters { get; } = new();

	/// <summary>
	///     Biases to apply to search, defaults to all properties set to null, which results in countrycode:auto bias, where
	///     geoapify will base its bias on the client IP.
	///     When using multiple biases, they're OR'ed together.
	///     <seealso cref="https://apidocs.geoapify.com/docs/geocoding/forward-geocoding/" />
	/// </summary>
	public Bias Biases { get; } = new();

	public IEnumerable<QueryStringValue> ToQueryString()
	{
		if (Type.HasValue)
		{
			yield return new QueryStringValue("type", Type.Value.ToString().ToLower());
		}

		yield return new QueryStringValue("lang", Language.GetDescription());

		if (Limit > 0)
		{
			yield return new QueryStringValue("limit", Limit.ToString());
		}

		var filterQueryStringValue = ((ISearchAreaComposer)Filters).ToQueryString();
		if (filterQueryStringValue is not null)
		{
			yield return filterQueryStringValue;
		}

		var biasQueryStringValue = ((ISearchAreaComposer)Biases).ToQueryString();
		if (biasQueryStringValue is not null)
		{
			yield return biasQueryStringValue;
		}
	}
}