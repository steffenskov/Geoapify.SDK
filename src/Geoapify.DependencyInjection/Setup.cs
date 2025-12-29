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