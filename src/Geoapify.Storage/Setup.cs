using Geoapify.DependencyInjection;
using Geoapify.Storage.Configuration;
using Geoapify.Storage.Services;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
public static class Setup
{
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
		if (services.ServiceCollection.Any(d => d.ImplementationType == typeof(StorageUpdaterService)))
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