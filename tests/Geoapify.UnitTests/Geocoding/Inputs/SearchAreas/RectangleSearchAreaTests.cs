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
}