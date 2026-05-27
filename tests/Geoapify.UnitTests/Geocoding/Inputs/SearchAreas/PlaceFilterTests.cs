using Geoapify.SDK.Geocoding.Inputs.SearchAreas;

namespace Geoapify.UnitTests.Geocoding.Inputs.SearchAreas;

public class PlaceSearchAreaTests
{
	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	public void ToQueryString_WhiteSpacePlaceId_Throws(string placeId)
	{
		// Act && Assert
		Assert.Throws<ArgumentException>(() => new PlaceSearchArea(placeId));
	}

	[Fact]
	public void ToQueryString_NullPlaceId_Throws()
	{
		// Act && Assert
		Assert.Throws<ArgumentNullException>(() => new PlaceSearchArea(null!));
	}

	[Fact]
	public void ToQueryString_ReturnsPlaceSearchArea()
	{
		// Arrange
		var searchArea = new PlaceSearchArea("51f076656f3e2a484059dc0a51be42c64b40f00101f901");

		// Act
		var queryString = ((ISearchArea)searchArea).ToQueryString();

		// Assert
		Assert.Equal("place:51f076656f3e2a484059dc0a51be42c64b40f00101f901", queryString);
	}
}