using Geoapify.SDK.Geocoding.Inputs.Biases;
using Geoapify.SDK.Geocoding.Inputs.SearchAreas;
using Geoapify.SDK.ValueObjects;

namespace Geoapify.UnitTests.Geocoding.Inputs.Biases;

public class BiasTests
{
	[Fact]
	public void QueryStringKey_ValidState_ReturnsBias()
	{
		// Arrange
		var bias = new Bias();

		// Act
		var queryStringKey = ((ISearchAreaComposer)bias).QueryStringKey;

		// Assert
		Assert.Equal("bias", queryStringKey);
	}

	[Fact]
	public void GetSearchAreas_OneOrMoreBiases_ReturnsSetBiases()
	{
		// Arrange
		var location = new LocationSearchArea(42, 13);
		var bias = new Bias
		{
			Location = location
		};

		// Act
		var searchAreas = ((ISearchAreaComposer)bias).GetSearchAreas();

		// Assert
		var returnedArea = Assert.Single(searchAreas);
		Assert.Equal(location, returnedArea);
	}

	[Fact]
	public void GetSearchAreas_AllBiasesSet_ReturnsAllBiases()
	{
		// Arrange
		var bias = new Bias
		{
			CountryCode = new CountryCodeSearchArea(CountryCode.Denmark),
			Location = new LocationSearchArea(42, 13),
			Circle = new CircleSearchArea(0, 0, 100),
			Rectangle = new RectangleSearchArea(0, 0, 1, 1)
		};

		// Act
		var searchAreas = ((ISearchAreaComposer)bias).GetSearchAreas();

		// Assert
		Assert.Equal(4, searchAreas.Count());
	}

	[Fact]
	public void GetSearchAreas_SomeBiasesSet_ReturnsSetBiases()
	{
		// Arrange
		var bias = new Bias
		{
			Location = new LocationSearchArea(42, 13),
			Circle = new CircleSearchArea(0, 0, 100)
		};

		// Act
		var searchAreas = ((ISearchAreaComposer)bias).GetSearchAreas();

		// Assert
		Assert.Equal(2, searchAreas.Count());
	}

	[Fact]
	public void GetSearchAreas_NoBiasesSet_ReturnsEmpty()
	{
		// Arrange
		var bias = new Bias();

		// Act
		var searchAreas = ((ISearchAreaComposer)bias).GetSearchAreas();

		// Assert
		Assert.Empty(searchAreas);
	}
}