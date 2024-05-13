using Server.Core.src.Entity;

namespace Server.Core.src.RepoAbstract
{
    public interface IWishlistRepo
    {
        Task<Wishlist> GetWishlistByIdAsync(Guid id);
        Task<IEnumerable<Wishlist>> GetWishlistByUserAsync(Guid userId);
        Task<bool> DeleteWishlistByIdAsync(Guid id);
        Task<Wishlist> CreateWishlistAsync(Wishlist wishlist);
        Task<Wishlist> UpdateWishlistByIdAsync(Wishlist wishlist);
        Task<bool> AddProductToWishlishAsync(Guid productId, Guid wishlistId);
        Task<bool> DeleteProductFromWishlishAsync(Guid productId, Guid wishlistId);
    }
}