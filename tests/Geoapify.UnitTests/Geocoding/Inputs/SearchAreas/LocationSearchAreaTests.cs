using Geoapify.SDK.Geocoding.Inputs.SearchAreas;

namespace Geoapify.UnitTests.Geocoding.Inputs.SearchAreas;

public class LocationSearchAreaTests
{
	[Fact]
	public void ToQueryString_Zeroes_ReturnsThat()
	{
		// Arrange
		var searchArea = new LocationSearchArea(0, 0);

		// Act
		var queryString = ((ISearchArea)searchArea).ToQueryString();

		// Assert
		Assert.Equal("proximity:0,0", queryString);
	}

	[Theory]
	[InlineData(90.01)]
	[InlineData(-90.01)]
	public void LocationSearchArea_LatitudeOutOfRange_Throws(double latitude)
	{
		// Act && Assert
		var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new LocationSearchArea(0, latitude));

		Assert.Contains("Latitude must be between -90.0 and 90.0", ex.Message);
		Assert.Equal("latitude", ex.ParamName);
	}

	[Theory]
	[InlineData(180.01)]
	[InlineData(-180.01)]
	public void LocationSearchArea_LongitudeOutOfRange_Throws(double longitude)
	{
		// Act && Assert
		var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new LocationSearchArea(longitude, 0));

		Assert.Contains("Longitude must be between -180.0 and 180.0", ex.Message);
		Assert.Equal("longitude", ex.ParamName);
	}
}