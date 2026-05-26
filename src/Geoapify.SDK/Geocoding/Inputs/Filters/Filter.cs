namespace Geoapify.SDK.Geocoding.Inputs.Filters;

public abstract class Filter
{
	internal abstract string ToQueryString();

	public static CountryCodeFilter ByCountryCodes(params ICollection<CountryCode> countryCodes)
	{
		return new CountryCodeFilter(countryCodes);
	}
}