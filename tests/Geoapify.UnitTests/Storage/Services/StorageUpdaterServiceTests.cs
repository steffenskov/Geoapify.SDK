using System.Diagnostics;
using Geoapify.SDK.Client;
using Geoapify.SDK.ReverseGeocoding;
using Geoapify.SDK.ReverseGeocoding.Inputs;
using Geoapify.SDK.Shared.Outputs;
using Geoapify.Storage.Configuration;
using Geoapify.Storage.Repositories;
using Geoapify.Storage.Services;
using Microsoft.Extensions.Logging;
using NSubstitute.ExceptionExtensions;

namespace Geoapify.UnitTests.Storage.Services;

public class StorageUpdaterServiceTests
{
	[Fact]
	public async Task ExecuteAsync_UpdateThrows_LogsAndContinues()
	{
		// Arrange
		var repository = Substitute.For<IAddressRepository>();
		var client = Substitute.For<IGeoapifyClient>();
		var options = Options.Create(new StorageUpdaterServiceConfiguration
		{
			LoopDelay = TimeSpan.Zero // No delay, should run in tight loop
		});
		var timeProvider = TimeProvider.System;
		var logger = Substitute.For<ILogger<StorageUpdaterService>>();

		repository.GetExpiredAsync(Arg.Any<DateTimeOffset>(), Arg.Any<CancellationToken>()).ThrowsAsync(new UnreachableException());

		var updaterService = new FakeStorageUpdaterService(repository, client, options, timeProvider, logger);
		var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(2));

		// Act
		await Assert.ThrowsAsync<TaskCanceledException>(async () => await updaterService.ExecuteAsyncPublic(cancellationTokenSource.Token));

		// Assert
		var logCalls = logger.ReceivedCalls()
			.Where(call => call.GetArguments().Any(arg => arg is UnreachableException));

		var logCount = logCalls.Count();

		Assert.True(logCount > 1);
	}

	[Fact]
	public async Task UpdateExpiredAddressesAsync_HasExpiredAddress_UpdatesItAndRaisesEvent()
	{
		// Arrange
		Address? addressFromChangedEvent = null;
		Address? addressUpserted = null;
		var repository = Substitute.For<IAddressRepository>();
		var client = Substitute.For<IGeoapifyClient>();
		var options = Options.Create(new StorageUpdaterServiceConfiguration());
		var timeProvider = TimeProvider.System;
		var logger = Substitute.For<ILogger<StorageUpdaterService>>();

		var reverseGeocoding = Substitute.For<IReverseGeocodingModule>();
		client.ReverseGeocoding.Returns(reverseGeocoding);

		reverseGeocoding.SearchAsync(Arg.Any<Coordinate>(), Arg.Any<ReverseGeocodingSearchArguments>(), Arg.Any<CancellationToken>())
			.Returns([
				new Address
				{
					Name = "Updated",
					LastUpdated = DateTimeOffset.UtcNow
				}
			]);

		repository.GetExpiredAsync(Arg.Any<DateTimeOffset>(), Arg.Any<CancellationToken>())
			.Returns([
				new Address
				{
					Name = "Expired",
					LastUpdated = DateTimeOffset.UtcNow.AddDays(-7)
				}
			]);

		repository.When(repo => repo.UpsertAsync(Arg.Any<Address>(), Arg.Any<CancellationToken>()))
			.Do(callInfo =>
			{
				addressUpserted = callInfo.Arg<Address>();
			});
		var updaterService = new FakeStorageUpdaterService(repository, client, options, timeProvider, logger);
		var addressChangedHandler = (Address address) =>
		{
			addressFromChangedEvent = address;
		};
		StorageUpdaterService.AddressChanged += addressChangedHandler;

		// Act
		try
		{
			await updaterService.UpdateExpiredAddressesAsyncPublic(TestContext.Current.CancellationToken);
		}
		finally
		{
			StorageUpdaterService.AddressChanged -= addressChangedHandler;
		}

		// Assert
		Assert.NotNull(addressFromChangedEvent);
		Assert.Equal("Updated", addressFromChangedEvent.Name);

		Assert.NotNull(addressUpserted);
		Assert.Equal("Updated", addressUpserted.Name);
	}

	[Fact]
	public async Task UpdateExpiredAddressesAsync_HasExpiredAddressWithoutChanges_UpsertsTheSameAddress()
	{
		// Arrange
		Address? addressFromChangedEvent = null;
		Address? addressUpserted = null;
		var repository = Substitute.For<IAddressRepository>();
		var client = Substitute.For<IGeoapifyClient>();
		var options = Options.Create(new StorageUpdaterServiceConfiguration());
		var timeProvider = TimeProvider.System;
		var logger = Substitute.For<ILogger<StorageUpdaterService>>();

		var reverseGeocoding = Substitute.For<IReverseGeocodingModule>();
		client.ReverseGeocoding.Returns(reverseGeocoding);

		reverseGeocoding.SearchAsync(Arg.Any<Coordinate>(), Arg.Any<ReverseGeocodingSearchArguments>(), Arg.Any<CancellationToken>())
			.Returns([
				new Address
				{
					Name = "Expired", // No changes
					LastUpdated = DateTimeOffset.UtcNow
				}
			]);

		repository.GetExpiredAsync(Arg.Any<DateTimeOffset>(), Arg.Any<CancellationToken>())
			.Returns([
				new Address
				{
					Name = "Expired",
					LastUpdated = DateTimeOffset.UtcNow.AddDays(-7)
				}
			]);

		repository.When(repo => repo.UpsertAsync(Arg.Any<Address>(), Arg.Any<CancellationToken>()))
			.Do(callInfo =>
			{
				addressUpserted = callInfo.Arg<Address>();
			});
		var updaterService = new FakeStorageUpdaterService(repository, client, options, timeProvider, logger);
		var addressChangedHandler = (Address address) =>
		{
			addressFromChangedEvent = address;
		};
		StorageUpdaterService.AddressChanged += addressChangedHandler;

		// Act
		try
		{
			await updaterService.UpdateExpiredAddressesAsyncPublic(TestContext.Current.CancellationToken);
		}
		finally
		{
			StorageUpdaterService.AddressChanged -= addressChangedHandler;
		}

		// Assert
		Assert.Null(addressFromChangedEvent);

		Assert.NotNull(addressUpserted);
		Assert.Equal("Expired", addressUpserted.Name);
	}

	[Fact]
	public async Task UpdateExpiredAddressesAsync_HasExpiredAddressThatIsObsolete_RetiresAddress()
	{
		// Arrange
		Address? addressFromChangedEvent = null;
		Address? addressUpserted = null;
		var repository = Substitute.For<IAddressRepository>();
		var client = Substitute.For<IGeoapifyClient>();
		var options = Options.Create(new StorageUpdaterServiceConfiguration());
		var timeProvider = TimeProvider.System;
		var logger = Substitute.For<ILogger<StorageUpdaterService>>();

		var reverseGeocoding = Substitute.For<IReverseGeocodingModule>();
		client.ReverseGeocoding.Returns(reverseGeocoding);

		reverseGeocoding.SearchAsync(Arg.Any<Coordinate>(), Arg.Any<ReverseGeocodingSearchArguments>(), Arg.Any<CancellationToken>())
			.Returns([]); // No result

		repository.GetExpiredAsync(Arg.Any<DateTimeOffset>(), Arg.Any<CancellationToken>())
			.Returns([
				new Address
				{
					Name = "Expired",
					LastUpdated = DateTimeOffset.UtcNow.AddDays(-7)
				}
			]);

		repository.When(repo => repo.UpsertAsync(Arg.Any<Address>(), Arg.Any<CancellationToken>()))
			.Do(callInfo =>
			{
				addressUpserted = callInfo.Arg<Address>();
			});
		var updaterService = new FakeStorageUpdaterService(repository, client, options, timeProvider, logger);
		var addressChangedHandler = (Address address) =>
		{
			addressFromChangedEvent = address;
		};
		StorageUpdaterService.AddressChanged += addressChangedHandler;

		// Act
		try
		{
			await updaterService.UpdateExpiredAddressesAsyncPublic(TestContext.Current.CancellationToken);
		}
		finally
		{
			StorageUpdaterService.AddressChanged -= addressChangedHandler;
		}

		// Assert
		Assert.Null(addressFromChangedEvent);

		Assert.NotNull(addressUpserted);
		Assert.Equal("Expired", addressUpserted.Name);
		Assert.True(addressUpserted.Retired);
	}
}

file class FakeStorageUpdaterService : StorageUpdaterService
{
	internal FakeStorageUpdaterService(IAddressRepository repository, IGeoapifyClient client, IOptions<StorageUpdaterServiceConfiguration> options, TimeProvider timeProvider, ILogger<StorageUpdaterService>? logger) : base(repository, client, options,
		timeProvider, logger)
	{
	}

	public async Task ExecuteAsyncPublic(CancellationToken cancellationToken)
	{
		await ExecuteAsync(cancellationToken);
	}


	public async Task UpdateExpiredAddressesAsyncPublic(CancellationToken cancellationToken)
	{
		await UpdateExpiredAddressesAsync(cancellationToken);
	}
}