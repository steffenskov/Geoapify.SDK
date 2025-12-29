using Geoapify.SDK.Client;
using Geoapify.SDK.ReverseGeocoding.Inputs;
using Geoapify.SDK.Shared.Outputs;
using Geoapify.Storage.Configuration;
using Geoapify.Storage.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Geoapify.Storage.Services;

public class StorageUpdaterService : BackgroundService
{
	private readonly IGeoapifyClient _client;
	private readonly ILogger<StorageUpdaterService>? _logger;
	private readonly TimeSpan _refreshDataAfter;
	private readonly IAddressRepository _repository;
	private readonly TimeProvider _timeProvider;

	internal StorageUpdaterService(IAddressRepository repository, IGeoapifyClient client, IOptions<StorageUpdaterServiceConfiguration> options, TimeProvider timeProvider, ILogger<StorageUpdaterService>? logger)
	{
		_repository = repository;
		_client = client;
		_timeProvider = timeProvider;
		_logger = logger;
		_refreshDataAfter = options.Value.RefreshDataAfter;
	}

	/// <summary>
	///     Event that's raised whenever the StorageUpdateService registers an address that was actually changed and stores
	///     that address.
	/// </summary>
	public static event Action<Address>? AddressChanged;

	protected async override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				await UpdateExpiredAddressesAsync(stoppingToken);
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, "Exception: {Message}", ex.Message);
			}

			await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
		}
	}

	private async Task UpdateExpiredAddressesAsync(CancellationToken cancellationToken)
	{
		var expirationDate = _timeProvider.GetUtcNow().Add(-_refreshDataAfter);

		var expiredAddresses = await _repository.GetExpiredAsync(expirationDate, cancellationToken);
		foreach (var expiredAddress in expiredAddresses)
		{
			var updatedAddress = (await _client.ReverseGeocoding.SearchAsync(expiredAddress.Coordinate, new ReverseGeocodingSearchArguments
				{
					Limit = 1
				},
				cancellationToken)).SingleOrDefault();

			if (updatedAddress is null)
			{
				// TODO: Mark address as EOL in repository so it no longer tries to update it
			}
			else
			{
				await _repository.UpsertAsync(updatedAddress, cancellationToken);
				if (updatedAddress.HasChanged(expiredAddress))
				{
					AddressChanged?.Invoke(updatedAddress);
				}
			}
		}
	}
}