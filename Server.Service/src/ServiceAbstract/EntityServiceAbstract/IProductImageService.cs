using Server.Core.src.Common;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.EntityServiceAbstract
{
    public interface IProductImageService
    {
        Task<IEnumerable<ProductImageReadDTO>> GetAllProductImagesAsync(Guid productId);
        Task<ProductImageReadDTO> GetProductImageById(Guid id);
        Task<ProductImageReadDTO> CreateProductImage(ProductImageCreateDTO prodImg);
        Task<ProductImageReadDTO> UpdateProductImage(Guid id, ProductImageUpdateDTO prodImg);
        Task<bool> DeleteProductImage(Guid id);
    }
}