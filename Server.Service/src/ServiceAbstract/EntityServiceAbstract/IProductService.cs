using Server.Core.src.Common;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.EntityServiceAbstract
{
    public interface IProductService
    {
        Task<IEnumerable<ProductReadDto>> GetAllProductsAsync(QueryOptions options);
        Task<int> GetProductsCountAsync(QueryOptions options);
        Task<ProductReadDto> GetProductByIdAsync(Guid id);
        Task<ProductReadDto> CreateProductAsync(ProductCreateDto prodImg);
        Task<ProductReadDto> UpdateProductAsync(Guid id, ProductUpdateDto prodImg);
        Task<bool> DeleteProductAsync(Guid id);
        Task<IEnumerable<ProductReadDto>> GetAllProductsByCategoryAsync(Guid categoryId);
        Task<IEnumerable<ProductReadDto>> GetAllProductsByCategoryAndSubcategoriesAsync(Guid categoryId);
        Task<IEnumerable<ProductReadDto>> GetMostPurchasedProductsAsync(int top);
        Task<IEnumerable<ProductReadDto>> GetTopRatedProductsAsync(int top);
    }
}