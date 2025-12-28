using Geoapify.SDK.Client;
using Geoapify.Storage.Repositories;
using Microsoft.Extensions.Hosting;

namespace Geoapify.Storage.Services;

public class StorageUpdaterService : BackgroundService
{
	private readonly IGeoapifyClient _client;
	private readonly IAddressRepository _repository;

	public StorageUpdaterService(IAddressRepository repository, IGeoapifyClient client)
	{
		_repository = repository;
		_client = client;
	}

	protected async override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			await UpdateExpiredAddressesAsync(stoppingToken);
			await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
		}
	}

	private async Task UpdateExpiredAddressesAsync(CancellationToken cancellationToken)
	{
		var expiredAddresses = await _repository.GetExpiredAsync(cancellationToken);
		foreach (var expiredAddress in expiredAddresses)
		{
			// TODO: Fetch updated data and upsert into repo
		}
	}
}