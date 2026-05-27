using Geoapify.SDK.Geocoding.Inputs.SearchAreas;
using Geoapify.SDK.ValueObjects;

namespace Geoapify.UnitTests.Geocoding.Inputs.SearchAreas;

public class CountryCodeSearchAreaTests
{
	[Fact]
	public void ToQueryString_NullCountryCode_Throws()
	{
		// Act && Assert
		var ex = Assert.Throws<ArgumentNullException>(() => new CountryCodeSearchArea(null!));
		Assert.Equal("countryCodes", ex.ParamName);
	}

	[Fact]
	public void ToQueryString_NoCountryCode_Throws()
	{
		// Act && Assert
		var ex = Assert.Throws<ArgumentException>(() => new CountryCodeSearchArea());

		Assert.Equal("At least one country code is required (Parameter 'countryCodes')", ex.Message);
	}

	[Fact]
	public void ToQueryString_SingleCountryCode_ReturnsSingleValue()
	{
		// Arrange
		var searchArea = new CountryCodeSearchArea(CountryCode.Denmark);

		// Act
		var queryString = ((ISearchArea)searchArea).ToQueryString();

		// Assert
		Assert.Equal("countrycode:dk", queryString);
	}

	[Fact]
	public void ToQueryString_MultipleCountryCodes_ReturnsCommaSeparated()
	{
		// Arrange
		var searchArea = new CountryCodeSearchArea(CountryCode.Denmark, CountryCode.United_Kingdom_of_Great_Britain_and_Northern_Ireland);

		// Act
		var queryString = ((ISearchArea)searchArea).ToQueryString();

		// Assert
		Assert.Equal("countrycode:dk,gb", queryString);
	}
}