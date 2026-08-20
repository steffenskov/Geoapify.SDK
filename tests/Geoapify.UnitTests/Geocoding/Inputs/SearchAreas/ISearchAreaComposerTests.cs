using Geoapify.SDK.Geocoding.Inputs.SearchAreas;
using Geoapify.SDK.ValueObjects;

namespace Geoapify.UnitTests.Geocoding.Inputs.SearchAreas;

public class ISearchAreaComposerTests
{
	[Fact]
	public void ToQueryString_NoSearchAreas_ReturnsNull()
	{
		// Arrange
		var composer = new FakeSearchAreaComposer();

		// Act
		var queryString = ((ISearchAreaComposer)composer).ToQueryString();

		// Assert
		Assert.Null(queryString);
	}

	[Fact]
	public void ToQueryString_SingleSearchArea_ReturnsQueryStringValue()
	{
		// Arrange
		var composer = new FakeSearchAreaComposer
		{
			CountryCode = new CountryCodeSearchArea(CountryCode.Denmark)
		};

		// Act
		var queryString = ((ISearchAreaComposer)composer).ToQueryString();

		// Assert
		Assert.NotNull(queryString);
		Assert.Equal("fake", queryString.Key);
		Assert.Equal("countrycode:dk", queryString.Value);
	}

	[Fact]
	public void ToQueryString_MultipleSearchAreas_JoinsSearchAreasWithPipe()
	{
		// Arrange
		var composer = new FakeSearchAreaComposer
		{
			CountryCode = new CountryCodeSearchArea(CountryCode.Denmark),
			Place = new PlaceSearchArea("51f076656f3e2a484059dc0a51be42c64b40f00101f901"),
			Circle = new CircleSearchArea(12.5, 55.7, 1000),
			Rectangle = new RectangleSearchArea(12.4, 55.6, 12.6, 55.8)
		};

		// Act
		var queryString = ((ISearchAreaComposer)composer).ToQueryString();

		// Assert
		Assert.NotNull(queryString);
		Assert.Equal("fake", queryString.Key);
		Assert.Equal(
			"countrycode:dk|place:51f076656f3e2a484059dc0a51be42c64b40f00101f901|circle:12.5,55.7,1000|rect:12.4,55.6,12.6,55.8",
			queryString.Value);
	}

	[Fact]
	public void GetSearchAreas_NoSearchAreas_ReturnsEmptyCollection()
	{
		// Arrange
		var composer = new FakeSearchAreaComposer();

		// Act
		var searchAreas = ((ISearchAreaComposer)composer).GetSearchAreas();

		// Assert
		Assert.Empty(searchAreas);
	}

	[Fact]
	public void GetSearchAreas_MultipleSearchAreas_ReturnsSearchAreasInExpectedOrder()
	{
		// Arrange
		var countryCode = new CountryCodeSearchArea(CountryCode.Denmark);
		var place = new PlaceSearchArea("51f076656f3e2a484059dc0a51be42c64b40f00101f901");
		var circle = new CircleSearchArea(12.5, 55.7, 1000);
		var rectangle = new RectangleSearchArea(12.4, 55.6, 12.6, 55.8);

		var composer = new FakeSearchAreaComposer
		{
			CountryCode = countryCode,
			Place = place,
			Circle = circle,
			Rectangle = rectangle
		};

		// Act
		var searchAreas = ((ISearchAreaComposer)composer).GetSearchAreas().ToArray();

		// Assert
		Assert.Equal([countryCode, place, circle, rectangle], searchAreas);
	}
}

file sealed class FakeSearchAreaComposer : ISearchAreaComposer
{
	public CountryCodeSearchArea? CountryCode { get; set; }
	public PlaceSearchArea? Place { get; set; }
	public CircleSearchArea? Circle { get; set; }
	public RectangleSearchArea? Rectangle { get; set; }
	public string QueryStringKey => "fake";

	IEnumerable<ISearchArea> ISearchAreaComposer.GetSearchAreas()
	{
		if (CountryCode is not null)
		{
			yield return CountryCode;
		}

		if (Place is not null)
		{
			yield return Place;
		}

		if (Circle is not null)
		{
			yield return Circle;
		}

		if (Rectangle is not null)
		{
			yield return Rectangle;
		}
	}
}