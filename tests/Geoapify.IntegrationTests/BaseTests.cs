using Geoapify.IntegrationTests.Configuration;
using Geoapify.SDK.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Geoapify.IntegrationTests;

[Collection(Consts.Fixture)]
public abstract class BaseTests
{
	protected readonly IGeoapifyClient _client;

	protected BaseTests(ContainerFixture fixture)
	{
		_client = fixture.Provider.GetRequiredService<IGeoapifyClient>();
	}
}