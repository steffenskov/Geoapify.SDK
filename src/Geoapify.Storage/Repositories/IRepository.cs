namespace Geoapify.Storage.Repositories;

public interface IRepository<in TId, TEntity>
{
	Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default);
	Task<TEntity> UpsertAsync(TEntity address, CancellationToken cancellationToken = default);
}