namespace Geoapify.SDK.ReverseGeocoding.Inputs;

public class ReverseGeocodingSearchArguments : IQueryStringArgument
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
	///     How many results to return, defaults to 1.
	/// </summary>
	public uint Limit { get; set; } = 1;

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
	}
}