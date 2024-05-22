using Server.Core.src.Common;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.EntityServiceAbstract
{
    public interface IProductService
    {
        Task<IEnumerable<ProductReadDto>> GetAllProductsAsync(QueryOptions options);
        Task<int> GetProductsCount(QueryOptions options);
        Task<ProductReadDto> GetProductById(Guid id);
        Task<ProductReadDto> CreateProduct(ProductCreateDto prodImg);
        Task<ProductReadDto> UpdateProduct(Guid id, ProductUpdateDto prodImg);
        Task<bool> DeleteProduct(Guid id);
        Task<IEnumerable<ProductReadDto>> GetAllProductsByCategoryAsync(Guid categoryId);
        Task<IEnumerable<ProductReadDto>> GetAllProductsByCategoryAndSubcategoriesAsync(Guid categoryId);
        Task<IEnumerable<ProductReadDto>> GetMostPurchasedProductsAsync(int top);
        Task<IEnumerable<ProductReadDto>> GetTopRatedProductsAsync(int top);
    }
}