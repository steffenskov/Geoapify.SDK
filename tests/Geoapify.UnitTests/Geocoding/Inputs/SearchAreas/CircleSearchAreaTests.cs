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
}