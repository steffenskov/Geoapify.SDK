using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Geoapify.IntegrationTests.Configuration;

public class ContainerFixture
{
	public ContainerFixture()
	{
		var services = new ServiceCollection();

		var configuration = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.AddUserSecrets(typeof(ContainerFixture).Assembly)
			.Build();

		var apiKey = configuration["ApiKey"] ?? throw new InvalidOperationException("Missing configuration value: ApiKey");

		services.AddGeoapify(apiKey);
		Provider = services.BuildServiceProvider();
	}

	public ServiceProvider Provider { get; }
}