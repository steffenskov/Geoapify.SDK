using Geoapify.DependencyInjection;
using Geoapify.SDK.Client;
using Geoapify.Storage.Configuration;
using Geoapify.Storage.Repositories;
using Geoapify.Storage.Services;

namespace Geoapify.UnitTests.Storage;

public class AddStorageUpdaterServiceTests
{
	[Fact]
	public void AddStorageUpdaterService_WithoutConfiguration_RegistersServiceWithDefaults()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddSingleton(Substitute.For<IAddressRepository>());
		services.AddSingleton(Substitute.For<IGeoapifyClient>());
		services.AddSingleton(TimeProvider.System);
		services.AddLogging();
		var geoapifyServices = new GeoapifyServiceCollection(services);

		// Act
		var result = geoapifyServices.AddStorageUpdaterService();

		// Assert
		Assert.Same(geoapifyServices, result); // Returns the same instance for chaining

		var serviceProvider = services.BuildServiceProvider();
		var hostedServices = serviceProvider.GetServices<IHostedService>();

		Assert.Contains(hostedServices, s => s.GetType() == typeof(StorageUpdaterService));

		var config = serviceProvider.GetRequiredService<IOptions<StorageUpdaterServiceConfiguration>>();
		Assert.NotNull(config.Value);
	}

	[Fact]
	public void AddStorageUpdaterService_WithConfiguration_RegistersServiceWithCustomConfig()
	{
		// Arrange
		var services = new ServiceCollection();
		var geoapifyServices = new GeoapifyServiceCollection(services);
		var expectedRefreshTime = TimeSpan.FromHours(2);
		var loopDelay = TimeSpan.FromMinutes(5);

		// Act
		var result = geoapifyServices.AddStorageUpdaterService(config =>
		{
			config.RefreshDataAfter = expectedRefreshTime;
			config.LoopDelay = loopDelay;
		});

		// Assert
		Assert.Same(geoapifyServices, result);

		var serviceProvider = services.BuildServiceProvider();
		var config = serviceProvider.GetRequiredService<IOptions<StorageUpdaterServiceConfiguration>>();

		Assert.Equal(expectedRefreshTime, config.Value.RefreshDataAfter);
		Assert.Equal(loopDelay, config.Value.LoopDelay);
	}

	[Fact]
	public void AddStorageUpdaterService_CalledTwice_RegistersServiceOnlyOnce()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddSingleton(Substitute.For<IAddressRepository>());
		services.AddSingleton(Substitute.For<IGeoapifyClient>());
		services.AddSingleton(TimeProvider.System);
		services.AddLogging();
		var geoapifyServices = new GeoapifyServiceCollection(services);

		// Act
		geoapifyServices.AddStorageUpdaterService();
		geoapifyServices.AddStorageUpdaterService(config =>
		{
			config.RefreshDataAfter = TimeSpan.FromHours(3);
		});

		// Assert
		var serviceProvider = services.BuildServiceProvider();
		var hostedServices = serviceProvider.GetServices<IHostedService>().ToList();

		var storageUpdaterServices = hostedServices.Where(s => s.GetType() == typeof(StorageUpdaterService));
		Assert.Single(storageUpdaterServices);
	}

	[Fact]
	public void AddStorageUpdaterService_CalledTwiceWithDifferentConfig_UsesFirstConfiguration()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddSingleton(Substitute.For<IAddressRepository>());
		services.AddSingleton(Substitute.For<IGeoapifyClient>());
		services.AddSingleton(TimeProvider.System);
		services.AddLogging();
		var geoapifyServices = new GeoapifyServiceCollection(services);
		var firstRefreshTime = TimeSpan.FromHours(1);
		var secondRefreshTime = TimeSpan.FromHours(3);

		// Act
		geoapifyServices.AddStorageUpdaterService(config =>
		{
			config.RefreshDataAfter = firstRefreshTime;
		});
		geoapifyServices.AddStorageUpdaterService(config =>
		{
			config.RefreshDataAfter = secondRefreshTime;
		});

		// Assert
		var serviceProvider = services.BuildServiceProvider();
		var config = serviceProvider.GetRequiredService<IOptions<StorageUpdaterServiceConfiguration>>();

		// The service should only be registered once
		var hostedServices = serviceProvider.GetServices<IHostedService>().ToList();
		var storageUpdaterServices = hostedServices.Where(s => s.GetType() == typeof(StorageUpdaterService)).ToList();
		Assert.Single(storageUpdaterServices);
		Assert.Equal(firstRefreshTime, config.Value.RefreshDataAfter); // First config wins
	}

	[Fact]
	public void AddStorageUpdaterService_RegistersAsHostedService()
	{
		// Arrange
		var services = new ServiceCollection();
		var geoapifyServices = new GeoapifyServiceCollection(services);

		// Act
		geoapifyServices.AddStorageUpdaterService();

		// Assert
		var descriptor = services.FirstOrDefault(d =>
			d.ServiceType == typeof(IHostedService) &&
			d.ImplementationType == typeof(StorageUpdaterService));

		Assert.NotNull(descriptor);
		Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
	}

	[Fact]
	public void AddStorageUpdaterService_SupportsMethodChaining()
	{
		// Arrange
		var services = new ServiceCollection();
		var geoapifyServices = new GeoapifyServiceCollection(services);

		// Act & Assert
		var result = geoapifyServices
			.AddStorageUpdaterService()
			.AddStorageUpdaterService(); // Should not throw and return same instance type

		Assert.NotNull(result);
		Assert.Same(geoapifyServices, result);
	}
}