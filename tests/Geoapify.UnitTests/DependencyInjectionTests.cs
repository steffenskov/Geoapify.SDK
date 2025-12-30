using Geoapify.DependencyInjection;
using Geoapify.SDK.Client;
using Geoapify.SDK.Configuration;

namespace Geoapify.UnitTests;

public class DependencyInjectionTests
{
	[Fact]
	public void AddGeoapify_RegistersGeoapifyClient()
	{
		// Arrange
		var services = new ServiceCollection();
		var apiKey = "test-api-key-123";

		// Act
		var result = services.AddGeoapify(apiKey);

		// Assert
		Assert.NotNull(result);
		Assert.IsType<GeoapifyServiceCollection>(result);

		var serviceProvider = services.BuildServiceProvider();
		var client = serviceProvider.GetService<IGeoapifyClient>();

		Assert.NotNull(client);
		Assert.IsType<GeoapifyClient>(client);
	}

	[Fact]
	public void AddGeoapify_ConfiguresApiKey()
	{
		// Arrange
		var services = new ServiceCollection();
		var apiKey = "my-secret-api-key";

		// Act
		services.AddGeoapify(apiKey);

		// Assert
		var serviceProvider = services.BuildServiceProvider();
		var configuration = serviceProvider.GetRequiredService<IOptions<GeoapifyConfiguration>>();

		Assert.Equal(apiKey, configuration.Value.ApiKey);
	}

	[Fact]
	public void AddGeoapify_RegistersTimeProvider()
	{
		// Arrange
		var services = new ServiceCollection();
		var apiKey = "test-api-key";

		// Act
		services.AddGeoapify(apiKey);

		// Assert
		var serviceProvider = services.BuildServiceProvider();
		var timeProvider = serviceProvider.GetService<TimeProvider>();

		Assert.NotNull(timeProvider);
		Assert.Same(TimeProvider.System, timeProvider);
	}

	[Fact]
	public void AddGeoapify_RegistersTimeProviderAsSingleton()
	{
		// Arrange
		var services = new ServiceCollection();
		var apiKey = "test-api-key";

		// Act
		services.AddGeoapify(apiKey);

		// Assert
		var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(TimeProvider));

		Assert.NotNull(descriptor);
		Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
	}

	[Fact]
	public void AddGeoapify_RegistersIGeoapifyClientAsTransient()
	{
		// Arrange
		var services = new ServiceCollection();
		var apiKey = "test-api-key";

		// Act
		services.AddGeoapify(apiKey);

		// Assert
		var descriptor = services.FirstOrDefault(d =>
			d.ServiceType == typeof(IGeoapifyClient) &&
			d.ImplementationType == typeof(GeoapifyClient));

		Assert.NotNull(descriptor);
		Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
	}

	[Fact]
	public void AddGeoapify_ConfiguresHttpClient()
	{
		// Arrange
		var services = new ServiceCollection();
		var apiKey = "test-api-key";

		// Act
		services.AddGeoapify(apiKey);

		// Assert
		var serviceProvider = services.BuildServiceProvider();
		var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
		var httpClient = httpClientFactory.CreateClient(nameof(GeoapifyClient));

		Assert.NotNull(httpClient);
		Assert.Equal(new Uri("https://api.geoapify.com/v1/"), httpClient.BaseAddress);
		Assert.Contains(httpClient.DefaultRequestHeaders.Accept,
			h => h.MediaType == "application/json");
		Assert.Contains("Geoapify.SDK/1.0", httpClient.DefaultRequestHeaders.UserAgent.ToString());
	}

	[Fact]
	public void AddGeoapify_ReturnsGeoapifyServiceCollection()
	{
		// Arrange
		var services = new ServiceCollection();
		var apiKey = "test-api-key";

		// Act
		var result = services.AddGeoapify(apiKey);

		// Assert
		Assert.NotNull(result);
		Assert.IsType<GeoapifyServiceCollection>(result);
		Assert.Same(services, result.ServiceCollection);
	}

	[Fact]
	public void AddGeoapify_CalledTwice_ThrowsInvalidOperationException()
	{
		// Arrange
		var services = new ServiceCollection();
		var apiKey1 = "test-api-key-1";
		var apiKey2 = "test-api-key-2";

		services.AddGeoapify(apiKey1);

		// Act & Assert
		var exception = Assert.Throws<InvalidOperationException>(() =>
			services.AddGeoapify(apiKey2));

		Assert.Equal("A Geoapify client is already registered.", exception.Message);
	}

	[Fact]
	public void AddGeoapify_WithEmptyApiKey_ThrowsArgumentNullException()
	{
		// Arrange
		var services = new ServiceCollection();
		var apiKey = string.Empty;

		// Act & Assert
		var exception = Assert.Throws<ArgumentNullException>(() =>
			services.AddGeoapify(apiKey));

		Assert.Equal("apiKey", exception.ParamName);
		Assert.Contains("Missing apiKey", exception.Message);
	}

	[Fact]
	public void AddGeoapify_WithNullApiKey_ThrowsArgumentNullException()
	{
		// Arrange
		var services = new ServiceCollection();

		// Act & Assert
		var exception = Assert.Throws<ArgumentNullException>(() =>
			services.AddGeoapify(null!));

		Assert.Equal("apiKey", exception.ParamName);
		Assert.Contains("Missing apiKey", exception.Message);
	}

	[Fact]
	public void AddGeoapify_WithWhitespaceApiKey_ThrowsArgumentNullException()
	{
		// Arrange
		var services = new ServiceCollection();
		var apiKey = "   ";

		// Act & Assert
		var exception = Assert.Throws<ArgumentNullException>(() =>
			services.AddGeoapify(apiKey));

		Assert.Equal("apiKey", exception.ParamName);
		Assert.Contains("Missing apiKey", exception.Message);
	}

	[Fact]
	public void AddGeoapify_CreatesTransientClientInstances()
	{
		// Arrange
		var services = new ServiceCollection();
		var apiKey = "test-api-key";
		services.AddGeoapify(apiKey);

		// Act
		var serviceProvider = services.BuildServiceProvider();
		var client1 = serviceProvider.GetRequiredService<IGeoapifyClient>();
		var client2 = serviceProvider.GetRequiredService<IGeoapifyClient>();

		// Assert
		Assert.NotNull(client1);
		Assert.NotNull(client2);
		Assert.NotSame(client1, client2); // Transient should create new instances
	}

	[Fact]
	public void AddGeoapify_HttpClientHasCorrectUserAgent()
	{
		// Arrange
		var services = new ServiceCollection();
		var apiKey = "test-api-key";

		// Act
		services.AddGeoapify(apiKey);

		// Assert
		var serviceProvider = services.BuildServiceProvider();
		var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
		var httpClient = httpClientFactory.CreateClient(nameof(GeoapifyClient));

		var userAgent = httpClient.DefaultRequestHeaders.UserAgent.ToString();
		Assert.Contains("Geoapify.SDK/1.0", userAgent);
		Assert.Contains("https://github.com/steffenskov/Geoapify.SDK", userAgent);
	}
}