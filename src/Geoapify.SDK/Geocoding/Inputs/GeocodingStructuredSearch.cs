namespace Geoapify.SDK.Geocoding.Inputs;

public class GeocodingStructuredSearch : IQueryStringArgument
{
	public string? Name { get; set; }
	public string? HouseNumber { get; set; }
	public string? Street { get; set; }
	public string? Postcode { get; set; }
	public string? City { get; set; }
	public string? State { get; set; }
	public string? Country { get; set; }


	public IEnumerable<QueryStringValue> ToQueryString()
	{
		if (!string.IsNullOrWhiteSpace(Name))
		{
			yield return new QueryStringValue("name", Name);
		}

		if (!string.IsNullOrWhiteSpace(HouseNumber))
		{
			yield return new QueryStringValue("housenumber", HouseNumber);
		}

		if (!string.IsNullOrWhiteSpace(Street))
		{
			yield return new QueryStringValue("street", Street);
		}

		if (!string.IsNullOrWhiteSpace(Postcode))
		{
			yield return new QueryStringValue("postcode", Postcode);
		}

		if (!string.IsNullOrWhiteSpace(City))
		{
			yield return new QueryStringValue("city", City);
		}

		if (!string.IsNullOrWhiteSpace(State))
		{
			yield return new QueryStringValue("state", State);
		}

		if (!string.IsNullOrWhiteSpace(Country))
		{
			yield return new QueryStringValue("country", Country);
		}
	}
}