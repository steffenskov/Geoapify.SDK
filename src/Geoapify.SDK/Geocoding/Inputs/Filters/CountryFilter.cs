namespace Geoapify.SDK.Geocoding.Inputs.Filters;

public class CountryFilter : Filter
{
	private readonly IEnumerable<CountryCode> _countryCodes;

	public CountryFilter(params IEnumerable<CountryCode> countryCodes)
	{
		_countryCodes = countryCodes;
	}

	override internal string ToQueryString()
	{
		return $"countrycode:{string.Join(",", _countryCodes.Select(code => code.GetDescription()))}";
	}
}