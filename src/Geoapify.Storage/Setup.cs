using Geoapify.DependencyInjection;
using Geoapify.Storage.Configuration;
using Geoapify.Storage.Services;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
public static class Setup
{
	private static bool _injected;

	/// <summary>
	///     Adds the StorageUpdaterService which will refresh data in your IAddressRepository periodically.
	/// </summary>
	/// <param name="services">Result from the .AddGeoapify() extension method</param>
	/// <param name="refreshDataAfter">
	///     How old data should be before being refreshed, do note that refreshing often will
	///     consume more Geoapify credits.
	/// </param>
	public static GeoapifyServiceCollection AddStorageUpdaterService(this GeoapifyServiceCollection services, TimeSpan refreshDataAfter)
	{
		var alreadyInjected = Interlocked.Exchange(ref _injected, true);
		if (alreadyInjected)
		{
			return services;
		}

		services.ServiceCollection.Configure<StorageUpdaterServiceConfiguration>(config =>
		{
			config.RefreshDataAfter = refreshDataAfter;
		});
		services.ServiceCollection.AddHostedService<StorageUpdaterService>();
		return services;
	}
}