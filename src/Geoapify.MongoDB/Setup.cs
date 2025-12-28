using Geoapify.MongoDB.Repositories;
using Geoapify.Storage.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

// ReSharper disable once CheckNamespace

public static class Setup
{
	// TODO: Consider making this an extension on top of a GeoapifyConfiguration object instead? Would enable single .AddGeoapify(config => config.AddMongoDB()) usage
	public static IServiceCollection AddGeoapifyMongoDBStorage(this IServiceCollection services, IMongoDatabase database, string addressCollectionName)
	{
		services.AddGeoapifyStorage();
		services.AddSingleton<IAddressRepository>(new AddressRepository(database, addressCollectionName));
		return services;
	}
}