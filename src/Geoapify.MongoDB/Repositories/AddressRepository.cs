using Geoapify.SDK.Geocoding;
using MongoDB.Driver;

namespace Geoapify.MongoDB.Repositories;

public class AddressRepository
{
	private readonly IMongoCollection<Address> _collection;

	public AddressRepository(IMongoDatabase db, string collectionName)
	{
		_collection = db.GetCollection<Address>(collectionName);
	}

	public Task<Address?> GetAsync(string id, CancellationToken cancellationToken)
	{
		var find = _collection.Find(e => e.PlaceId == id).FirstOrDefaultAsync();
		return find;
	}
}