using Geoapify.SDK.Geocoding.Inputs.SearchAreas;

namespace Geoapify.UnitTests.Geocoding.Inputs.SearchAreas;

public class CircleSearchAreaTests
{
	[Fact]
	public void ToQueryString_ZeroRadius_Throws()
	{
		// Act && Assert
		var ex = Assert.Throws<ArgumentException>(() => new CircleSearchArea(12.5, 55.7, 0));

		Assert.Equal("Radius must be greater than 0 (Parameter 'radiusInMeters')", ex.Message);
	}

	[Fact]
	public void ToQueryString_ReturnsCircleFilter()
	{
		// Arrange
		var searchArea = new CircleSearchArea(12.5, 55.7, 1000);

		// Act
		var queryString = ((ISearchArea)searchArea).ToQueryString();

		// Assert
		Assert.Equal("circle:12.5,55.7,1000", queryString);
	}


	[Theory]
	[InlineData(90.01)]
	[InlineData(-90.01)]
	public void CircleSearchArea_LatitudeOutOfRange_Throws(double latitude)
	{
		// Act && Assert
		var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new CircleSearchArea(0, latitude, 1));

		Assert.Contains("Latitude must be between -90.0 and 90.0", ex.Message);
		Assert.Equal("latitude", ex.ParamName);
	}

	[Theory]
	[InlineData(180.01)]
	[InlineData(-180.01)]
	public void CircleSearchArea_LongitudeOutOfRange_Throws(double longitude)
	{
		// Act && Assert
		var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new CircleSearchArea(longitude, 0, 1));

		Assert.Contains("Longitude must be between -180.0 and 180.0", ex.Message);
		Assert.Equal("longitude", ex.ParamName);
	}
}