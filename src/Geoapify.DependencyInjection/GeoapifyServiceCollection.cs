using Microsoft.Extensions.DependencyInjection;

namespace Geoapify.DependencyInjection;

public class GeoapifyServiceCollection
{
	internal GeoapifyServiceCollection(IServiceCollection serviceCollection)
	{
		ServiceCollection = serviceCollection;
	}

	public IServiceCollection ServiceCollection { get; }
}