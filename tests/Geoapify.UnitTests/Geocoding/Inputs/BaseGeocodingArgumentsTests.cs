using Geoapify.SDK.Geocoding.Inputs;
using Geoapify.SDK.Geocoding.Inputs.SearchAreas;
using Geoapify.SDK.ValueObjects;

namespace Geoapify.UnitTests.Geocoding.Inputs;

public class FakeGeocodingArgumentsTests
{
	[Fact]
	public void ToQueryString_SingleFilter_IncludesFilter()
	{
		// Arrange
		var arguments = new FakeGeocodingArguments
		{
			Filters =
			{
				CountryCode = new CountryCodeSearchArea(CountryCode.Denmark)
			}
		};

		// Act
		var queryString = arguments.ToQueryString().ToList();

		// Assert
		var filterArgument = queryString.Single(qs => qs.Key == "filter");

		Assert.Equal("countrycode:dk", filterArgument.Value);
	}

	[Fact]
	public void ToQueryString_WithFilters_IncludesPipedFilters()
	{
		// Arrange
		var arguments = new FakeGeocodingArguments
		{
			Filters =
			{
				CountryCode = new CountryCodeSearchArea(CountryCode.Denmark, CountryCode.United_Kingdom_of_Great_Britain_and_Northern_Ireland),
				Place = new PlaceSearchArea("fake")
			}
		};

		// Act
		var queryString = arguments.ToQueryString().ToList();

		// Assert
		var filterArgument = queryString.Single(qs => qs.Key == "filter");

		Assert.Equal("countrycode:dk,gb|place:fake", filterArgument.Value);
	}

	[Fact]
	public void ToQueryString_NoFilters_OutputWithoutFilter()
	{
		// Arrange
		var arguments = new FakeGeocodingArguments();

		// Act
		var queryString = arguments.ToQueryString().ToList();

		// Assert
		Assert.DoesNotContain(queryString, qs => qs.Key == "filter");
	}

	[Fact]
	public void ToQueryString_SingleBias_IncludesBias()
	{
		// Arrange
		var arguments = new FakeGeocodingArguments
		{
			Biases =
			{
				CountryCode = new CountryCodeSearchArea(CountryCode.Denmark)
			}
		};

		// Act
		var queryString = arguments.ToQueryString().ToList();

		// Assert
		var filterArgument = queryString.Single(qs => qs.Key == "bias");

		Assert.Equal("countrycode:dk", filterArgument.Value);
	}

	[Fact]
	public void ToQueryString_WithBiases_IncludesPipedBiases()
	{
		// Arrange
		var arguments = new FakeGeocodingArguments
		{
			Biases =
			{
				CountryCode = new CountryCodeSearchArea(CountryCode.Denmark, CountryCode.United_Kingdom_of_Great_Britain_and_Northern_Ireland),
				Location = new LocationSearchArea(42, 13.37)
			}
		};

		// Act
		var queryString = arguments.ToQueryString().ToList();

		// Assert
		var filterArgument = queryString.Single(qs => qs.Key == "bias");

		Assert.Equal("countrycode:dk,gb|proximity:42,13.37", filterArgument.Value);
	}

	[Fact]
	public void ToQueryString_NoBiases_OutputWithoutBias()
	{
		// Arrange
		var arguments = new FakeGeocodingArguments();

		// Act
		var queryString = arguments.ToQueryString().ToList();

		// Assert
		Assert.DoesNotContain(queryString, qs => qs.Key == "bias");
	}
}

file sealed class FakeGeocodingArguments : BaseGeocodingArguments
{
}