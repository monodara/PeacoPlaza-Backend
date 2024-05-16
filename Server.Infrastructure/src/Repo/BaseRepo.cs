using Microsoft.EntityFrameworkCore;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Infrastructure.src.Database;

namespace Server.Infrastructure.src.Repo;

public class BaseRepo<T> : IBaseRepo<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    protected readonly DbSet<T> _data;

    public BaseRepo(AppDbContext context)
    {
        _context = context;
        _data = _context.Set<T>();
    }

    public async Task<T> CreateOneAsync(T createObject)
    {
        await _data.AddAsync(createObject);
        await _context.SaveChangesAsync();
        return createObject;
    }

    public async Task<bool> DeleteOneByIdAsync(T deleteObject)
    {
        _data.Remove(deleteObject);
        await _context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(QueryOptions options)
    {
        return await _data.ToListAsync();
    }

    public virtual async Task<T> GetOneByIdAsync(Guid id)
    {
        var data = await _data.FirstOrDefaultAsync(c => c.Id == id);
        return data;
    }

    public async Task<T> UpdateOneByIdAsync(T updateObject)
    {
        _data.Update(updateObject);
        await _context.SaveChangesAsync();
        return updateObject;
    }
}