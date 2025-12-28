using Geoapify.Storage.Services;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
public static class Setup
{
	private static bool _injected;

	// TODO: Consider making this an extension on top of a GeoapifyConfiguration object instead? Would enable single .AddGeoapify(config => config.AddStorage()) usage
	public static IServiceCollection AddGeoapifyStorage(this IServiceCollection services)
	{
		var alreadyInjected = Interlocked.Exchange(ref _injected, true);
		if (alreadyInjected)
		{
			return services;
		}

		services.AddHostedService<StorageUpdaterService>();
		return services;
	}
}