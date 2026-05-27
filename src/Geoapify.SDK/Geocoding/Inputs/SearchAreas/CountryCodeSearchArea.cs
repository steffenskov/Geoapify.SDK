namespace Geoapify.SDK.Geocoding.Inputs.SearchAreas;

public sealed class CountryCodeSearchArea : ISearchArea
{
	private readonly ICollection<CountryCode> _countryCodes;

	public CountryCodeSearchArea(params ICollection<CountryCode> countryCodes)
	{
		ArgumentNullException.ThrowIfNull(countryCodes);
		if (countryCodes.Count == 0)
		{
			throw new ArgumentException("At least one country code is required", nameof(countryCodes));
		}

		_countryCodes = countryCodes;
	}

	string ISearchArea.ToQueryString()
	{
		return $"countrycode:{string.Join(",", _countryCodes.Select(code => code.GetDescription()))}";
	}
}