using Geoapify.SDK.Shared;

namespace Geoapify.UnitTests.Shared;

public class AddressIdTests
{
	[Fact]
	public void Create_FixedInput_ReturnsSameOutput()
	{
		// Arrange
		var latitude = Random.Shared.NextDouble();
		var longitude = Random.Shared.NextDouble();

		// Act
		var id1 = AddressId.Create(latitude, longitude);
		var id2 = AddressId.Create(latitude, longitude);

		// Assert
		Assert.Equal(id1, id2);
	}

	[Fact]
	public void Parse_WasCreatedFromCoordinates_ExtractsSameCoordinates()
	{
		// Arrange
		var latitude = Random.Shared.NextDouble();
		var longitude = Random.Shared.NextDouble();
		var id = AddressId.Create(latitude, longitude);

		// Act
		var (parsedLat, parsedLong) = AddressId.Parse(id);

		// Assert
		Assert.Equal(latitude, parsedLat);
		Assert.Equal(longitude, parsedLong);
	}
}