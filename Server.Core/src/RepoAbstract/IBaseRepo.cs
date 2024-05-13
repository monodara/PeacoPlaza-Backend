using Server.Core.src.Entity;
using Server.Core.src.Common;

namespace Server.Core.src.RepoAbstract;

public interface IBaseRepo<T> where T : BaseEntity
{
    public Task<IEnumerable<T>> GetAllAsync(QueryOptions options);
    public Task<T> GetOneByIdAsync(Guid id);
    public Task<T> CreateOneAsync(T createObject);
    public Task<T> UpdateOneByIdAsync(T updateObject);
    public Task<bool> DeleteOneByIdAsync(T deleteObject);
}