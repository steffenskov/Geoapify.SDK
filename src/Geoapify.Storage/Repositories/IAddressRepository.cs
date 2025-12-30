using Geoapify.SDK.Shared.Outputs;

namespace Geoapify.Storage.Repositories;

/// <summary>
///     Represents a database repository for addresses.
/// </summary>
public interface IAddressRepository : IRepository<Guid, Address>
{
	/// <summary>
	///     Returns all non-retired expired addresses
	/// </summary>
	/// <param name="expirationDate">Date used to determine expiration</param>
	/// <param name="cancellationToken">cancellationToken</param>
	Task<IEnumerable<Address>> GetExpiredAsync(DateTimeOffset expirationDate, CancellationToken cancellationToken = default);
}