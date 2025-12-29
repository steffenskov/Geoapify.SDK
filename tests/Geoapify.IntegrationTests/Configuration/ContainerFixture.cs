using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace Geoapify.IntegrationTests.Configuration;

public class ContainerFixture : IAsyncLifetime
{
	private readonly MongoDbContainer _mongoContainer;

	public ContainerFixture()
	{
		_mongoContainer = new MongoDbBuilder()
			.WithImage("mongo:latest")
			.WithUsername("mongo")
			.WithPassword("secret")
			.Build();
	}

	public ServiceProvider Provider { get; private set; } = null!;

	public async ValueTask InitializeAsync()
	{
		await _mongoContainer.StartAsync();

		var services = new ServiceCollection();

		var configuration = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.AddUserSecrets(typeof(ContainerFixture).Assembly)
			.Build();

		var apiKey = configuration["ApiKey"] ?? throw new InvalidOperationException("Missing configuration value: ApiKey");

		var client = new MongoClient(_mongoContainer.GetConnectionString());
		var db = client.GetDatabase("db");
		services.AddGeoapify(apiKey)
			.AddMongoDBStorage(db, "addresses");
		Provider = services.BuildServiceProvider();
	}

	public async ValueTask DisposeAsync()
	{
		await _mongoContainer.DisposeAsync();
	}
}