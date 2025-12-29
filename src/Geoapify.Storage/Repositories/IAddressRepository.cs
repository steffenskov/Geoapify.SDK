using Geoapify.SDK.Shared.Outputs;

namespace Geoapify.Storage.Repositories;

/// <summary>
///     Represents a database repository for addresses.
/// </summary>
public interface IAddressRepository : IRepository<Guid, Address>
{
	Task<IEnumerable<Address>> GetExpiredAsync(DateTimeOffset expirationDate, CancellationToken cancellationToken = default);
}