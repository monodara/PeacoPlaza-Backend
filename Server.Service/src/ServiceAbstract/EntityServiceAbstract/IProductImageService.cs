using Server.Core.src.Common;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.EntityServiceAbstract
{
    public interface IProductImageService
    {
        Task<IEnumerable<ProductImageReadDto>> GetAllProductImagesAsync(Guid productId);
        Task<ProductImageReadDto> GetProductImageById(Guid id);
        Task<ProductImageReadDto> CreateProductImage(ProductImageCreateDto prodImg);
        Task<ProductImageReadDto> UpdateProductImage(Guid id, ProductImageUpdateDto prodImg);
        Task<bool> DeleteProductImage(Guid id);
    }
}