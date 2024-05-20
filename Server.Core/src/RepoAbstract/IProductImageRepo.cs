using Server.Core.src.Entity;

namespace Server.Core.src.RepoAbstract;

public interface IProductImageRepo : IBaseRepo<ProductImage>
{
    public Task<IEnumerable<ProductImage>> GetImageByProductAsync(Guid productId);
}