namespace Geoapify.Storage.Repositories;

public interface IRepository<in TId, TEntity>
{
	/// <summary>
	///     Gets a single entity based on id, if it exists - otherwise returns null.
	/// </summary>
	/// <param name="id">id of the entity to get</param>
	/// <param name="cancellationToken">cancellationToken</param>
	/// <returns>Entity with the given id or null if it does not exist</returns>
	Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default);

	/// <summary>
	///     Upserts an entity either inserting it or replacing an existing one based on id.
	/// </summary>
	/// <param name="entity">entity to upsert</param>
	/// <param name="cancellationToken">cancellationToken</param>
	/// <returns>The upserted entity</returns>
	Task<TEntity> UpsertAsync(TEntity entity, CancellationToken cancellationToken = default);
}