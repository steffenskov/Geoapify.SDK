using Geoapify.SDK.Geocoding.Inputs.Filters;
using Geoapify.SDK.ValueObjects;

namespace Geoapify.UnitTests.Geocoding.Inputs.Filters;

public class CountryCodeFilterTests
{
	[Fact]
	public void ToQueryString_NullCountryCode_Throws()
	{
		// Act && Assert
		var ex = Assert.Throws<ArgumentNullException>(() => new CountryCodeFilter(null!));
		Assert.Equal("countryCodes", ex.ParamName);
	}

	[Fact]
	public void ToQueryString_NoCountryCode_Throws()
	{
		// Act && Assert
		var ex = Assert.Throws<ArgumentException>(() => new CountryCodeFilter());

		Assert.Equal("At least one country code is required (Parameter 'countryCodes')", ex.Message);
	}

	[Fact]
	public void ToQueryString_SingleCountryCode_ReturnsSingleValue()
	{
		// Arrange
		var filter = new CountryCodeFilter(CountryCode.Denmark);

		// Act
		var queryString = filter.ToQueryString();

		// Assert
		Assert.Equal("countrycode:dk", queryString);
	}

	[Fact]
	public void ToQueryString_MultipleCountryCodes_ReturnsCommaSeparated()
	{
		// Arrange
		var filter = new CountryCodeFilter(CountryCode.Denmark, CountryCode.United_Kingdom_of_Great_Britain_and_Northern_Ireland);

		// Act
		var queryString = filter.ToQueryString();

		// Assert
		Assert.Equal("countrycode:dk,gb", queryString);
	}
}