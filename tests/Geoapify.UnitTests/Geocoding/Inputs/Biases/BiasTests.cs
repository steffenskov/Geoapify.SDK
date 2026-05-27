using Geoapify.SDK.Geocoding.Inputs.Biases;
using Geoapify.SDK.Geocoding.Inputs.SearchAreas;

namespace Geoapify.UnitTests.Geocoding.Inputs.Biases;

public class BiasTests
{
	[Fact]
	public void QueryStringKey_ValidState_ReturnsBias()
	{
		// Arrange
		var filter = new Bias();

		// Act
		var queryStringKey = ((ISearchAreaComposer)filter).QueryStringKey;

		// Assert
		Assert.Equal("bias", queryStringKey);
	}
}