namespace Geoapify.SDK.Geocoding.Inputs.Filters;

public abstract class Filter
{
	internal abstract string ToQueryString();

	public static CountryFilter ByCountry(params IEnumerable<CountryCode> countryCodes)
	{
		return new CountryFilter(countryCodes);
	}
}