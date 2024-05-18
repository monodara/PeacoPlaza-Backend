using Server.Core.src.Common;
using Server.Core.src.Entity;

namespace Server.Core.src.RepoAbstract;

public interface IProductRepo : IBaseRepo<Product>
{
    IEnumerable<Product> GetByCategory(Guid categoryId);
    Task<IEnumerable<Product>> GetMostPurchasedAsync(int topNumber);
}