using Microsoft.IdentityModel.Tokens;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;

namespace Server.Service.src.ServiceImplement.EntityServiceImplement
{

    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepo _wishlistRepo;

        public WishlistService(IWishlistRepo wishlistRepo)
        {
            _wishlistRepo = wishlistRepo;
        }
        public async Task<bool> AddProductToWishlishAsync(Guid productId, Guid wishlistId)
        {
            return await _wishlistRepo.AddProductToWishlishAsync(productId, wishlistId);
        }

        public async Task<WishlistReadDto> CreateWishlistAsync(Guid userId, WishlistCreateDto wishlist)
        {
            // to do : duplicated wishlist name
            if (string.IsNullOrEmpty(wishlist.Name))
                throw new ArgumentNullException("Name cannot be empty!");
            var wishlistToAdd = wishlist.CreateWishlist(userId);
            var addedWishlist = await _wishlistRepo.CreateWishlistAsync(wishlistToAdd);
            return new WishlistReadDto().Transform(addedWishlist);
        }

        public async Task<bool> DeleteProductFromWishlishAsync(Guid productId, Guid wishlistId)
        {
            return await _wishlistRepo.DeleteProductFromWishlishAsync(productId, wishlistId);
        }

        public async Task<bool> DeleteWishlistByIdAsync(Guid id)
        {
            var isDeleted = await _wishlistRepo.DeleteWishlistByIdAsync(id);
            if (!isDeleted)
            {
                throw new ResourceNotFoundException("Wishlist is not found.");
            }
            return true;
        }

        public async Task<WishlistReadDto> GetWishlistByIdAsync(Guid id)
        {
            var wishlist = await _wishlistRepo.GetWishlistByIdAsync(id);
            if (wishlist == null)
            {
                throw new ResourceNotFoundException("No wishlist found by this id.");
            }
            return new WishlistReadDto().Transform(wishlist);
        }

        public async Task<IEnumerable<WishlistReadDto>> GetWishlistByUserAsync(Guid userId)
        {
            var wishlists = await _wishlistRepo.GetWishlistByUserAsync(userId);
            return wishlists.Select(wl => new WishlistReadDto().Transform(wl));
        }

        public async Task<WishlistReadDto> UpdateWishlistByIdAsync(Guid id, WishlistUpdateDto wishlist)
        {
            var wishlistToUpdate = await _wishlistRepo.GetWishlistByIdAsync(id);
            if (wishlistToUpdate == null)
            {
                throw new ResourceNotFoundException("No wishlist found to update.");
            }
            var wishlistNewInfo = wishlist.UpdateWishlist(wishlistToUpdate);
            var updatedWishlist = await _wishlistRepo.UpdateWishlistByIdAsync(wishlistNewInfo);
            if (updatedWishlist == null)
            {
                throw new InvalidOperationException("Updating wishlist failed.");
            }
            return new WishlistReadDto().Transform(updatedWishlist);
        }
    }
}