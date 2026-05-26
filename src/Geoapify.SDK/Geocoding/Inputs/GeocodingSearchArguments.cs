using Geoapify.SDK.Geocoding.Inputs.Filters;

namespace Geoapify.SDK.Geocoding.Inputs;

public class GeocodingSearchArguments : IQueryStringArgument
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
	///     Filters to apply to search, defaults to empty, which means no filtering.
	///     Possible Filter implementations can be found in the <see cref="Geoapify.SDK.Geocoding.Inputs.Filters" /> namespace
	///     When using multiple filters, they're AND'ed together.
	/// </summary>
	public Filter[] Filters { get; set; } = [];

	// TODO: Add Bias

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

		if (Filters.Length > 0)
		{
			var filterValue = string.Join("|", Filters.Select(filter => filter.ToQueryString()));
			yield return new QueryStringValue("filter", filterValue);
		}
	}
}