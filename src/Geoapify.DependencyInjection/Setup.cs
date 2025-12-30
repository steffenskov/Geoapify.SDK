using System.Net;
using Geoapify.DependencyInjection;
using Geoapify.SDK.Client;
using Geoapify.SDK.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
public static class Setup
{
	public static GeoapifyServiceCollection AddGeoapify(this IServiceCollection services, string apiKey)
	{
		if (string.IsNullOrWhiteSpace(apiKey))
		{
			throw new ArgumentNullException(nameof(apiKey), "Missing apiKey");
		}

		if (services.Any(d => d.ImplementationType == typeof(GeoapifyClient)))
		{
			throw new InvalidOperationException("A Geoapify client is already registered.");
		}

		services.AddHttpClient<GeoapifyClient>(client =>
		{
			client.BaseAddress = new Uri("https://api.geoapify.com/v1/");
			client.DefaultRequestHeaders.Add("Accept", "application/json");
			client.DefaultRequestHeaders.UserAgent.ParseAdd("Geoapify.SDK/1.0 (+https://github.com/steffenskov/Geoapify.SDK)");
		}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
		{
			AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
			AllowAutoRedirect = true
		});
		services.Configure<GeoapifyConfiguration>(options =>
		{
			options.ApiKey = apiKey;
		});
		services.AddSingleton(TimeProvider.System);
		services.AddTransient<IGeoapifyClient, GeoapifyClient>();
		return new GeoapifyServiceCollection(services);
	}
}