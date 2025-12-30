using Geoapify.IntegrationTests.Configuration;
using Geoapify.SDK.ReverseGeocoding.Inputs;
using Geoapify.SDK.ValueObjects;

namespace Geoapify.IntegrationTests.ReverseGeocoding;

public class ReverseGeocodingModuleTests : BaseTests
{
	public ReverseGeocodingModuleTests(ContainerFixture fixture) : base(fixture)
	{
	}

	[Fact]
	public async Task SearchAsync_LatLongExists_ReturnsAddress()
	{
		// Arrange
		var latitude = 56.441651999999998;
		var longitude = 9.3864319999999992;

		// Act
		var result = (await _client.ReverseGeocoding.SearchAsync(latitude, longitude, cancellationToken: TestContext.Current.CancellationToken)).ToList();

		// Assert
		Assert.NotEmpty(result);
		Assert.Single(result);
		Assert.Equal("""
		             Falkevej 40
		             8800 Viborg, Denmark
		             """, result[0].AddressLines);
	}

	[Fact]
	public async Task SearchAsync_LatLongInOpenSea_ReturnsEmpty()
	{
		// Arrange
		var latitude = 56.316610;
		var longitude = 11.346151;

		var filter = new ReverseGeocodingSearchArguments
		{
			Type = LocationTypes.Street
		};

		// Act
		var result = (await _client.ReverseGeocoding.SearchAsync(latitude, longitude, filter, TestContext.Current.CancellationToken)).ToList();

		// Assert
		Assert.Empty(result);
	}
}