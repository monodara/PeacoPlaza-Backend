using Server.Core.src.Entity;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.EntityServiceAbstract
{
    public interface IWishlistService
    {
        Task<WishlistReadDto> GetWishlistByIdAsync(Guid id);
        Task<IEnumerable<WishlistReadDto>> GetWishlistByUserAsync(Guid userId);
        Task<bool> DeleteWishlistByIdAsync(Guid id);
        Task<WishlistReadDto> CreateWishlistAsync(Guid userId, WishlistCreateDto wishlist);
        Task<WishlistReadDto> UpdateWishlistByIdAsync(WishlistUpdateDto wishlist);
        Task<bool> AddProductToWishlishAsync(Guid productId, Guid wishlistId);
        Task<bool> DeleteProductFromWishlishAsync(Guid productId, Guid wishlistId);
    }
}