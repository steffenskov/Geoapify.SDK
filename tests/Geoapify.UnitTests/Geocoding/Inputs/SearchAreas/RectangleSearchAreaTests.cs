using Geoapify.SDK.Geocoding.Inputs.SearchAreas;

namespace Geoapify.UnitTests.Geocoding.Inputs.SearchAreas;

public class RectangleSearchAreaTests
{
	[Fact]
	public void ToQueryString_ReturnsRectangleFilter()
	{
		// Arrange
		var searchArea = new RectangleSearchArea(12.4, 55.6, 12.6, 55.8);

		// Act
		var queryString = ((ISearchArea)searchArea).ToQueryString();

		// Assert
		Assert.Equal("rect:12.4,55.6,12.6,55.8", queryString);
	}

	[Theory]
	[InlineData(90.01)]
	[InlineData(-90.01)]
	public void RectangleSearchArea_Latitude1OutOfRange_Throws(double latitude)
	{
		// Act && Assert
		var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new RectangleSearchArea(0, latitude, 0, 0));

		Assert.Contains("Latitude must be between -90.0 and 90.0", ex.Message);
		Assert.Equal("latitude1", ex.ParamName);
	}

	[Theory]
	[InlineData(180.01)]
	[InlineData(-180.01)]
	public void RectangleSearchArea_Longitude1OutOfRange_Throws(double longitude)
	{
		// Act && Assert
		var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new RectangleSearchArea(longitude, 0, 0, 0));

		Assert.Contains("Longitude must be between -180.0 and 180.0", ex.Message);
		Assert.Equal("longitude1", ex.ParamName);
	}

	[Theory]
	[InlineData(90.01)]
	[InlineData(-90.01)]
	public void RectangleSearchArea_Latitude2OutOfRange_Throws(double latitude)
	{
		// Act && Assert
		var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new RectangleSearchArea(0, 0, 0, latitude));

		Assert.Contains("Latitude must be between -90.0 and 90.0", ex.Message);
		Assert.Equal("latitude2", ex.ParamName);
	}

	[Theory]
	[InlineData(180.01)]
	[InlineData(-180.01)]
	public void RectangleSearchArea_Longitude2OutOfRange_Throws(double longitude)
	{
		// Act && Assert
		var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new RectangleSearchArea(0, 0, longitude, 0));

		Assert.Contains("Longitude must be between -180.0 and 180.0", ex.Message);
		Assert.Equal("longitude2", ex.ParamName);
	}
}