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
	private readonly IEnumerable<IAddressChangedHandler> _changeHandlers;
	private readonly IGeoapifyClient _client;
	private readonly StorageUpdaterServiceConfiguration _configuration;
	private readonly ILogger<StorageUpdaterService>? _logger;
	private readonly IAddressRepository _repository;
	private readonly TimeProvider _timeProvider;

	public StorageUpdaterService(IAddressRepository repository, IGeoapifyClient client, IOptions<StorageUpdaterServiceConfiguration> options, TimeProvider timeProvider, IEnumerable<IAddressChangedHandler> changeHandlers,
		ILogger<StorageUpdaterService>? logger)
	{
		_repository = repository;
		_client = client;
		_timeProvider = timeProvider;
		_changeHandlers = changeHandlers;
		_logger = logger;
		_configuration = options.Value;
	}

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
					Language = expiredAddress.Language,
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
					await NotifyHandlersAsync(updatedAddress);
				}
			}
		}
	}

	private async Task NotifyHandlersAsync(Address updatedAddress)
	{
		foreach (var handler in _changeHandlers)
		{
			try
			{
				await handler.HandleAsync(updatedAddress);
			}
			catch (Exception ex)
			{
				var handlerType = handler.GetType();
				_logger?.LogError(ex, "Exception when invoking {Handler}: {Message}", handlerType.FullName ?? handlerType.Name, ex.Message);
			}
		}
	}
}