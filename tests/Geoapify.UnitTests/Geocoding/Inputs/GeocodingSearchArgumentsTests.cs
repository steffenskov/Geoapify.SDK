using Geoapify.SDK.Geocoding.Inputs;
using Geoapify.SDK.Geocoding.Inputs.Filters;
using Geoapify.SDK.ValueObjects;

namespace Geoapify.UnitTests.Geocoding.Inputs;

public class GeocodingSearchArgumentsTests
{
	[Fact]
	public void ToQueryString_SingleFilter_IncludesFilter()
	{
		// Arrange
		var arguments = new GeocodingSearchArguments
		{
			Filters =
			[
				Filter.ByCountry(CountryCode.Denmark)
			]
		};

		// Act
		var queryString = arguments.ToQueryString().ToArray();

		// Assert
		var filterArgument = queryString.Single(qs => qs.Key == "filter");

		Assert.Equal("countrycode:dk", filterArgument.Value);
	}


	[Fact]
	public void ToQueryString_WithFilters_IncludesPipedFilters()
	{
		// Arrange
		var arguments = new GeocodingSearchArguments
		{
			Filters =
			[
				Filter.ByCountry(CountryCode.Denmark, CountryCode.United_Kingdom_of_Great_Britain_and_Northern_Ireland),
				new FakeFilter()
			]
		};

		// Act
		var queryString = arguments.ToQueryString().ToArray();

		// Assert
		var filterArgument = queryString.Single(qs => qs.Key == "filter");

		Assert.Equal("countrycode:dk,gb|fake:filter", filterArgument.Value);
	}

	[Fact]
	public void ToQueryString_NoFilters_OutputWithoutFilter()
	{
		// Arrange
		var arguments = new GeocodingSearchArguments();

		// Act
		var queryString = arguments.ToQueryString().ToArray();

		// Assert
		Assert.DoesNotContain(queryString, qs => qs.Key == "filter");
	}
}

file sealed class FakeFilter : Filter
{
	override internal string ToQueryString()
	{
		return "fake:filter";
	}
}