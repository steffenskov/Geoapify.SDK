using Geoapify.SDK.Shared.Outputs;

namespace Geoapify.UnitTests.Shared.Outputs;

public class AddressTests
{
	[Fact]
	public void HasChanged_OnlyLastUpdatedIsDifferent_ReturnsFalse()
	{
		// Arrange
		var oldAddress = new Address
		{
			Id = Guid.NewGuid(),
			Coordinate = new Coordinate
			{
				Latitude = 13.37,
				Longitude = 10.24
			},
			LastUpdated = DateTimeOffset.UtcNow.AddDays(-2)
		};

		var newAddress = oldAddress with
		{
			Coordinate = new Coordinate // Set to a new object with same values to verify the entire comparison is value based, including nested objects
			{
				Latitude = 13.37,
				Longitude = 10.24
			},
			LastUpdated = DateTimeOffset.UtcNow
		};

		// Act
		var hasChanged = newAddress.HasChanged(oldAddress);

		// Assert
		Assert.False(hasChanged);
	}

	[Fact]
	public void HasChanged_SomethingChanged_ReturnsTrue()
	{
		// Arrange
		var oldAddress = new Address
		{
			Id = Guid.NewGuid(),
			Coordinate = new Coordinate
			{
				Latitude = 13.37,
				Longitude = 10.24
			},
			LastUpdated = DateTimeOffset.UtcNow.AddDays(-2)
		};

		var newAddress = oldAddress with
		{
			Coordinate = new Coordinate
			{
				Latitude = 13.37,
				Longitude = 10.23
			}
		};

		// Act
		var hasChanged = newAddress.HasChanged(oldAddress);

		// Assert
		Assert.True(hasChanged);
	}
}