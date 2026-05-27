using Geoapify.SDK.Geocoding.Inputs.Filters;
using Geoapify.SDK.Geocoding.Inputs.SearchAreas;

namespace Geoapify.UnitTests.Geocoding.Inputs.Filters;

public class FilterTests
{
	[Fact]
	public void QueryStringKey_ValidState_ReturnsFilter()
	{
		// Arrange
		var filter = new Filter();

		// Act
		var queryStringKey = ((ISearchAreaComposer)filter).QueryStringKey;

		// Assert
		Assert.Equal("filter", queryStringKey);
	}
}