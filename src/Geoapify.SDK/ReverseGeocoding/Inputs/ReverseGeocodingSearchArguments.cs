using Geoapify.SDK.ValueObjects;

namespace Geoapify.SDK.ReverseGeocoding.Inputs;

public class ReverseGeocodingSearchArguments : IQueryStringArgument
{
	public LocationTypes? Type { get; set; }
	public Language? Language { get; set; }
	public uint Limit { get; set; } = 1;

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

		if (Limit > 0)
		{
			yield return new QueryStringValue("limit", Limit.ToString());
		}
	}
}