using Geoapify.IntegrationTests.Configuration;
using Geoapify.SDK.Geocoding.Outputs;
using Geoapify.Storage.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Geoapify.IntegrationTests.Storage.MongoDB;

public class AddressRepositoryTests : BaseTests
{
	public AddressRepositoryTests(ContainerFixture fixture) : base(fixture)
	{
	}

	[Fact]
	public async Task UpsertAsync_ValidAddress_IsUpserted()
	{
		// Arrange
		var repository = Provider.GetRequiredService<IAddressRepository>();
		var id = Guid.CreateVersion7().ToString();
		var address = new Address
		{
			Id = id,
			LastUpdated = DateTimeOffset.UtcNow.AddDays(-2)
		};

		// Act
		await repository.UpsertAsync(address, TestContext.Current.CancellationToken);

		// Assert
		var fetched = await repository.GetAsync(id, TestContext.Current.CancellationToken);

		Assert.NotNull(fetched);
		Assert.Equal(address.LastUpdated, fetched.LastUpdated);
	}

	[Fact]
	public async Task GetAsync_InvalidId_ReturnsNull()
	{
		// Arrange
		var repository = Provider.GetRequiredService<IAddressRepository>();
		var id = Guid.CreateVersion7().ToString();

		// Act
		var result = await repository.GetAsync(id, TestContext.Current.CancellationToken);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetExpiredAsync_SomeExpiredAddressesExist_ReturnsJustThose()
	{
		// Arrange
		var repository = Provider.GetRequiredService<IAddressRepository>();
		var expiredId = Guid.CreateVersion7().ToString();
		var nonExpiredId = Guid.CreateVersion7().ToString();
		var expiredAddress = new Address
		{
			Id = expiredId,
			LastUpdated = DateTimeOffset.UtcNow.AddDays(-8)
		};
		var nonExpiredAddress = new Address
		{
			Id = nonExpiredId,
			LastUpdated = DateTimeOffset.UtcNow.AddDays(-2)
		};

		await repository.UpsertAsync(expiredAddress, TestContext.Current.CancellationToken);
		await repository.UpsertAsync(nonExpiredAddress, TestContext.Current.CancellationToken);

		// Act
		var result = (await repository.GetExpiredAsync(TestContext.Current.CancellationToken)).ToList();

		// Assert
		Assert.NotEmpty(result);
		Assert.Contains(result, address => address.Id == expiredId);
		Assert.DoesNotContain(result, address => address.Id == nonExpiredId);
	}
}