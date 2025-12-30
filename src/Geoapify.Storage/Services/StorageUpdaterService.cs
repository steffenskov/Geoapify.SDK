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
	private readonly StorageUpdaterServiceConfiguration _configuration;
	private readonly ILogger<StorageUpdaterService>? _logger;
	private readonly IAddressRepository _repository;
	private readonly TimeProvider _timeProvider;

	public StorageUpdaterService(IAddressRepository repository, IGeoapifyClient client, IOptions<StorageUpdaterServiceConfiguration> options, TimeProvider timeProvider, ILogger<StorageUpdaterService>? logger)
	{
		_repository = repository;
		_client = client;
		_timeProvider = timeProvider;
		_logger = logger;
		_configuration = options.Value;
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

			await Task.Delay(_configuration.LoopDelay, stoppingToken);
		}
	}

	protected async Task UpdateExpiredAddressesAsync(CancellationToken cancellationToken)
	{
		var expirationDate = _timeProvider.GetUtcNow().Add(-_configuration.RefreshDataAfter);

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
				await _repository.UpsertAsync(expiredAddress.Retire(), cancellationToken);
			}
			else
			{
				await _repository.UpsertAsync(updatedAddress, cancellationToken);
				if (updatedAddress.HasChanged(expiredAddress))
				{
					AddressChanged?.Invoke(updatedAddress); // TODO: Consider using something else for raising events? This is cumbersome to unsubscribe from
				}
			}
		}
	}
}