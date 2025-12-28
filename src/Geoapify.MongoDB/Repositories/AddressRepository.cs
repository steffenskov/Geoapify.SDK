using Geoapify.SDK.Geocoding.Outputs;
using Geoapify.Storage.Repositories;
using MongoDB.Driver;

namespace Geoapify.MongoDB.Repositories;

public class AddressRepository : IAddressRepository
{
	private readonly IMongoCollection<Address> _collection;

	public AddressRepository(IMongoDatabase db, string collectionName)
	{
		_collection = db.GetCollection<Address>(collectionName);
		_collection.Indexes.CreateOne(new CreateIndexModel<Address>(Builders<Address>.IndexKeys.Ascending(x => x.LastUpdated)));
	}

	public async Task<Address?> GetAsync(string id, CancellationToken cancellationToken)
	{
		var find = _collection.Find(e => e.Id == id);

		return await find.SingleOrDefaultAsync(cancellationToken);
	}

	public async Task<Address> UpsertAsync(Address address, CancellationToken cancellationToken)
	{
		await _collection.ReplaceOneAsync(e => e.Id == address.Id, address,
			new ReplaceOptions { IsUpsert = true },
			cancellationToken);
		return address;
	}

	public async Task<IEnumerable<Address>> GetExpiredAsync(CancellationToken cancellationToken = default)
	{
		var find = _collection.Find(e => e.LastUpdated < DateTimeOffset.UtcNow.AddDays(-7));

		return await find.ToListAsync(cancellationToken);
	}
}