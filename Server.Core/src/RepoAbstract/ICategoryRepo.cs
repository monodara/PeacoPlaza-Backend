using Server.Core.src.Entity;

namespace Server.Core.src.RepoAbstract;

public interface ICategoryRepo : IBaseRepo<Category>
{
    Task<IEnumerable<Category>> GetSubcategories(Guid cateId);
}