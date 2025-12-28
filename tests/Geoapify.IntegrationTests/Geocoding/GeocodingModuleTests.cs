using Geoapify.IntegrationTests.Configuration;

namespace Geoapify.IntegrationTests.Geocoding;

public class GeocodingModuleTests : BaseTests
{
	public GeocodingModuleTests(ContainerFixture fixture) : base(fixture)
	{
	}

	[Fact]
	public async Task SearchAsync_ValidAddress_ReturnsAddress()
	{
		// Arrange
		var address = "Falkevej 40, 8800 Viborg, Danmark";

		// Act
		var result = (await _client.Geocoding.SearchAsync(address, TestContext.Current.CancellationToken)).ToList();

		// Assert
		Assert.NotEmpty(result);
		Assert.Single(result);
	}
}