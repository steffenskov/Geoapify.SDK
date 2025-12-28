using Geoapify.SDK.ValueObjects;

namespace Geoapify.SDK.Geocoding.Inputs;

public class GeocodingSearchArguments : IQueryStringArgument
{
	public LocationTypes? Type { get; set; }
	public Language? Language { get; set; }
	public uint Limit { get; set; } = 5;

	// TODO: Add Filter
	// TODO: Add Bias

	public IEnumerable<QueryStringValue> ToQueryString()
	{
		if (Type.HasValue)
		{
			yield return new QueryStringValue("type", Type.Value.ToString().ToLower());
		}

		if (Language.HasValue)
		{
			yield return new QueryStringValue("lang", Language.Value.GetDescription());
		}

		yield return new QueryStringValue("limit", Limit.ToString());
	}
}