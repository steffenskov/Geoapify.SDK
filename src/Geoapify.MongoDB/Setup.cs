using Geoapify.DependencyInjection;
using Geoapify.MongoDB.Repositories;
using Geoapify.Storage.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

// ReSharper disable once CheckNamespace

public static class Setup
{
	/// <summary>
	///     Adds Mongo DB support for local storage of addresses.
	/// </summary>
	/// <param name="services">Result from the .AddGeoapify() extension method</param>
	/// <param name="database">Mongo database to use</param>
	/// <param name="addressCollectionName">Name of Mongo collection store addresses in</param>
	/// <param name="registerGuidSerializer">
	///     Whether to register a GuidSerializer for you with GuidRepresentation.Standard or
	///     not. Defaults to true.
	/// </param>
	public static GeoapifyServiceCollection AddMongoDBStorage(this GeoapifyServiceCollection services, IMongoDatabase database, string addressCollectionName, bool registerGuidSerializer = true)
	{
		if (services.ServiceCollection.Any(d => d.ServiceType == typeof(IAddressRepository)))
		{
			throw new InvalidOperationException("An IAddressRepository service is already registered.");
		}

		if (registerGuidSerializer)
		{
			BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
		}

		services.ServiceCollection.AddSingleton<IAddressRepository>(new AddressRepository(database, addressCollectionName));
		return services;
	}
}