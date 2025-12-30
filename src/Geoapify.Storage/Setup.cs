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
	/// <param name="configure">
	///     Configuration of the service
	/// </param>
	public static GeoapifyServiceCollection AddStorageUpdaterService(this GeoapifyServiceCollection services, Action<StorageUpdaterServiceConfiguration>? configure = null)
	{
		if (services.ServiceCollection.Any(d => d.ImplementationType == typeof(StorageUpdaterService)))
		{
			return services;
		}

		if (configure is not null)
		{
			services.ServiceCollection.Configure(configure);
		}
		else
		{
			services.ServiceCollection.Configure<StorageUpdaterServiceConfiguration>(_ => // Use default values
			{
			});
		}

		services.ServiceCollection.AddHostedService<StorageUpdaterService>();
		return services;
	}
}