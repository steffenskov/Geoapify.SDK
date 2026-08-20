using Geoapify.SDK.Geocoding.Inputs.Filters;
using Geoapify.SDK.Geocoding.Inputs.SearchAreas;
using Geoapify.SDK.ValueObjects;

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


	[Fact]
	public void GetSearchAreas_AllFiltersSet_ReturnsAllFilters()
	{
		// Arrange
		var filter = new Filter
		{
			CountryCode = new CountryCodeSearchArea(CountryCode.Denmark),
			Place = new PlaceSearchArea("København"),
			Circle = new CircleSearchArea(0, 0, 100),
			Rectangle = new RectangleSearchArea(0, 0, 1, 1)
		};

		// Act
		var searchAreas = ((ISearchAreaComposer)filter).GetSearchAreas();

		// Assert
		Assert.Equal(4, searchAreas.Count());
	}

	[Fact]
	public void GetSearchAreas_SomeFiltersSet_ReturnsSetFilters()
	{
		// Arrange
		var filter = new Filter
		{
			Circle = new CircleSearchArea(0, 0, 100),
			Rectangle = new RectangleSearchArea(0, 0, 1, 1)
		};

		// Act
		var searchAreas = ((ISearchAreaComposer)filter).GetSearchAreas();

		// Assert
		Assert.Equal(2, searchAreas.Count());
	}

	[Fact]
	public void GetSearchAreas_NoFiltersSet_ReturnsEmpty()
	{
		// Arrange
		var filter = new Filter();

		// Act
		var searchAreas = ((ISearchAreaComposer)filter).GetSearchAreas();

		// Assert
		Assert.Empty(searchAreas);
	}
}