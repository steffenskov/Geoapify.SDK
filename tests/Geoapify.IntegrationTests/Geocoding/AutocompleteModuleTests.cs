using Geoapify.IntegrationTests.Configuration;
using Geoapify.SDK.Geocoding.Inputs;
using Geoapify.SDK.Geocoding.Inputs.SearchAreas;
using Geoapify.SDK.ValueObjects;

namespace Geoapify.IntegrationTests.Geocoding;

public class AutocompleteModuleTests : BaseTests
{
	public AutocompleteModuleTests(ContainerFixture fixture) : base(fixture)
	{
	}

	[Fact]
	public async Task AutocompleteAsync_ExactAddress_ReturnsAtLeastOneAddress()
	{
		// Arrange
		var address = "Falkevej 40, 8800 Viborg, Danmark";

		// Act
		var result = (await _client.Autocomplete.AutocompleteAsync(address, cancellationToken: TestContext.Current.CancellationToken)).ToList();

		// Assert
		Assert.NotEmpty(result);
	}

	[Fact]
	public async Task AutocompleteAsync_PartialTextInDenmark_ReturnsEmpty()
	{
		// Arrange
		var address = "Vejl";

		// Act
		var result = (await _client.Autocomplete.AutocompleteAsync(address, new GeocodingAutocompleteArguments
		{
			Filters =
			{
				CountryCode = new CountryCodeSearchArea(CountryCode.Denmark)
			}
		}, TestContext.Current.CancellationToken)).ToList();

		// Assert
		Assert.NotEmpty(result);
		Assert.All(result, a => Assert.Equal("dk", a.CountryCode, true));
	}
}