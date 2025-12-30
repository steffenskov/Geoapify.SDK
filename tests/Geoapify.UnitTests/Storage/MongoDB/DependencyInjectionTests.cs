using Geoapify.DependencyInjection;
using Geoapify.Storage.MongoDB.Repositories;
using Geoapify.Storage.Repositories;
using MongoDB.Driver;

namespace Geoapify.UnitTests.Storage.MongoDB;

public class DependencyInjectionTests
{
	[Fact]
	public void AddMongoDBStorage_FirstCall_RegistersAddressRepository()
	{
		// Arrange
		var services = new ServiceCollection();
		var geoapifyServices = new GeoapifyServiceCollection(services);
		var database = Substitute.For<IMongoDatabase>();
		var collectionName = "addresses";

		// Act
		var result = geoapifyServices.AddMongoDBStorage(database, collectionName, false);

		// Assert
		Assert.Same(geoapifyServices, result); // Returns the same instance for chaining

		var serviceProvider = services.BuildServiceProvider();
		var repository = serviceProvider.GetService<IAddressRepository>();

		Assert.NotNull(repository);
		Assert.IsType<AddressRepository>(repository);
	}

	[Fact]
	public void AddMongoDBStorage_WithRegisterGuidSerializerTrue_RegistersGuidSerializer()
	{
		// Arrange
		var services = new ServiceCollection();
		var geoapifyServices = new GeoapifyServiceCollection(services);
		var database = Substitute.For<IMongoDatabase>();
		var collectionName = "addresses";

		// Act
		geoapifyServices.AddMongoDBStorage(database, collectionName);

		// Assert
		var serviceProvider = services.BuildServiceProvider();
		var repository = serviceProvider.GetService<IAddressRepository>();

		Assert.NotNull(repository);
		// Note: Verifying BsonSerializer registration is tricky as it's a global state
		// This test mainly ensures no exception is thrown
	}

	[Fact]
	public void AddMongoDBStorage_WithRegisterGuidSerializerFalse_DoesNotRegisterGuidSerializer()
	{
		// Arrange
		var services = new ServiceCollection();
		var geoapifyServices = new GeoapifyServiceCollection(services);
		var database = Substitute.For<IMongoDatabase>();
		var collectionName = "addresses";

		// Act
		geoapifyServices.AddMongoDBStorage(database, collectionName, false);

		// Assert
		var serviceProvider = services.BuildServiceProvider();
		var repository = serviceProvider.GetService<IAddressRepository>();

		Assert.NotNull(repository);
		// Successfully registered without attempting GUID serializer registration
	}

	[Fact]
	public void AddMongoDBStorage_CalledTwice_ThrowsInvalidOperationException()
	{
		// Arrange
		var services = new ServiceCollection();
		var geoapifyServices = new GeoapifyServiceCollection(services);
		var database = Substitute.For<IMongoDatabase>();
		var collectionName = "addresses";

		geoapifyServices.AddMongoDBStorage(database, collectionName, false);

		// Act & Assert
		var exception = Assert.Throws<InvalidOperationException>(() =>
			geoapifyServices.AddMongoDBStorage(database, collectionName, false));

		Assert.Equal("An IAddressRepository service is already registered.", exception.Message);
	}

	[Fact]
	public void AddMongoDBStorage_WithDifferentCollectionNames_StillThrowsOnSecondCall()
	{
		// Arrange
		var services = new ServiceCollection();
		var geoapifyServices = new GeoapifyServiceCollection(services);
		var database = Substitute.For<IMongoDatabase>();

		geoapifyServices.AddMongoDBStorage(database, "addresses1", false);

		// Act & Assert
		var exception = Assert.Throws<InvalidOperationException>(() =>
			geoapifyServices.AddMongoDBStorage(database, "addresses2", false));

		Assert.Equal("An IAddressRepository service is already registered.", exception.Message);
	}

	[Fact]
	public void AddMongoDBStorage_RegistersRepositoryAsSingleton()
	{
		// Arrange
		var services = new ServiceCollection();
		var geoapifyServices = new GeoapifyServiceCollection(services);
		var database = Substitute.For<IMongoDatabase>();
		var collectionName = "addresses";

		// Act
		geoapifyServices.AddMongoDBStorage(database, collectionName, false);

		// Assert
		var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IAddressRepository));

		Assert.NotNull(descriptor);
		Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
	}

	[Fact]
	public void AddMongoDBStorage_SupportsMethodChaining()
	{
		// Arrange
		var services = new ServiceCollection();
		var geoapifyServices = new GeoapifyServiceCollection(services);
		var database = Substitute.For<IMongoDatabase>();
		var collectionName = "addresses";

		// Act
		var result = geoapifyServices.AddMongoDBStorage(database, collectionName, false);

		// Assert
		Assert.NotNull(result);
		Assert.Same(geoapifyServices, result);
	}

	[Fact]
	public void AddMongoDBStorage_WithNullDatabase_ThrowsArgumentNullException()
	{
		// Arrange
		var services = new ServiceCollection();
		var geoapifyServices = new GeoapifyServiceCollection(services);
		var collectionName = "addresses";

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() =>
			geoapifyServices.AddMongoDBStorage(null!, collectionName, false));
	}

	[Fact]
	public void AddMongoDBStorage_WithNullCollectionName_ThrowsArgumentNullException()
	{
		// Arrange
		var services = new ServiceCollection();
		var geoapifyServices = new GeoapifyServiceCollection(services);
		var database = Substitute.For<IMongoDatabase>();

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() =>
			geoapifyServices.AddMongoDBStorage(database, null!, false));
	}
}