namespace Geoapify.SDK.Geocoding.Inputs.Filters;

public class CountryCodeFilter : Filter
{
	private readonly ICollection<CountryCode> _countryCodes;

	public CountryCodeFilter(params ICollection<CountryCode> countryCodes)
	{
		ArgumentNullException.ThrowIfNull(countryCodes);
		if (countryCodes.Count == 0)
		{
			throw new ArgumentException("At least one country code is required", nameof(countryCodes));
		}

		_countryCodes = countryCodes;
	}

	override internal string ToQueryString()
	{
		return $"countrycode:{string.Join(",", _countryCodes.Select(code => code.GetDescription()))}";
	}
}