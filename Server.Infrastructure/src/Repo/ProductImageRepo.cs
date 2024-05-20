using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Infrastructure.src.Database;

namespace Server.Infrastructure.src.Repo;
public class ProductImageRepo : BaseRepo<ProductImage>, IProductImageRepo
{
    public ProductImageRepo(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProductImage>> GetImageByProductAsync(Guid productId)
    {
        var query = _data.Where(img => img.ProductId == productId);
        return await Task.FromResult(query.ToList());
    }
}