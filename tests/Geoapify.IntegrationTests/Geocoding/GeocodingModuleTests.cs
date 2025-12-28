using Geoapify.IntegrationTests.Configuration;
using Geoapify.SDK.Geocoding.Inputs;
using Geoapify.SDK.ValueObjects;

namespace Geoapify.IntegrationTests.Geocoding;

public class GeocodingModuleTests : BaseTests
{
	public GeocodingModuleTests(ContainerFixture fixture) : base(fixture)
	{
	}

	[Fact]
	public async Task SearchAsync_FreeText_ReturnsAddress()
	{
		// Arrange
		var address = "Falkevej 40, 8800 Viborg, Danmark";

		// Act
		var result = (await _client.Geocoding.SearchAsync(address, cancellationToken: TestContext.Current.CancellationToken)).ToList();

		// Assert
		Assert.NotEmpty(result);
		Assert.Single(result);
	}

	[Fact]
	public async Task SearchAsync_Structured_ReturnsAddress()
	{
		// Arrange
		var model = new GeocodingStructuredSearch
		{
			City = "Viborg",
			Postcode = "8800",
			Country = "Danmark",
			Street = "Falkevej",
			HouseNumber = "40"
		};

		// Act
		var result = (await _client.Geocoding.SearchAsync(model, cancellationToken: TestContext.Current.CancellationToken)).ToList();

		// Assert
		Assert.NotEmpty(result);
		Assert.Single(result);
	}

	[Fact]
	public async Task SearchAsync_StructuredWithLanguage_ReturnsLocalized()
	{
		// Arrange
		var model = new GeocodingStructuredSearch
		{
			City = "Viborg",
			Postcode = "8800",
			Country = "Danmark",
			Street = "Falkevej",
			HouseNumber = "40"
		};

		var arguments = new GeocodingSearchArguments
		{
			Language = Language.Danish
		};

		// Act
		var result = (await _client.Geocoding.SearchAsync(model, arguments, TestContext.Current.CancellationToken)).ToList();

		// Assert
		Assert.NotEmpty(result);
		var foundAddress = result.Single();
		Assert.Equal("Viborg Kommune", foundAddress.Municipality);
	}

	[Fact]
	public async Task SearchAsync_StructuredWithType_FindsMultiple()
	{
		// Arrange
		var model = new GeocodingStructuredSearch
		{
			City = "Viborg",
			Postcode = "8800",
			Country = "Danmark",
			Street = "Falkevej",
			HouseNumber = "40"
		};

		var arguments = new GeocodingSearchArguments
		{
			Type = LocationTypes.Locality
		};

		// Act
		var result = (await _client.Geocoding.SearchAsync(model, arguments, TestContext.Current.CancellationToken)).ToList();

		// Assert
		Assert.NotEmpty(result);
		Assert.True(result.Count > 1);
	}
}