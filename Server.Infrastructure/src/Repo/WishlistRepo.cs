using Microsoft.EntityFrameworkCore;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Infrastructure.src.Database;

namespace Server.Infrastructure.src.Repo
{
    public class WishlistRepo : IWishlistRepo
    {
        private readonly AppDbContext _context;
        // private readonly User _user;
        public WishlistRepo(AppDbContext context)
        {
            _context = context;
            // _user = user;
        }
        public async Task<bool> AddProductToWishlishAsync(Guid productId, Guid wishlistId)
        {
            await _context.WishlistItems.AddAsync(new WishlistItem(productId, wishlistId));
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Wishlist> CreateWishlistAsync(Wishlist wishlist)
        {
            await _context.Wishlists.AddAsync(wishlist);
            await _context.SaveChangesAsync();
            return wishlist;
        }

        public async Task<bool> DeleteProductFromWishlishAsync(Guid productId, Guid wishlistId)
        {
            var wishlistItemToRemove = await _context.WishlistItems.FirstOrDefaultAsync(wli => wli.ProductId == productId && wli.WishlistId == wishlistId);
            if (wishlistItemToRemove != null)
            {
                _context.WishlistItems.Remove(wishlistItemToRemove);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteWishlistByIdAsync(Guid id)
        {
            var wishlistToDelete = await GetWishlistByIdAsync(id);
            if (wishlistToDelete != null)
            {
                _context.Wishlists.Remove(wishlistToDelete);
                await _context.SaveChangesAsync();
                return true; // Return true indicating successful deletion
            }
            else
            {
                return false; // Return false indicating user not found or deletion failed
            }
        }

        public async Task<Wishlist> GetWishlistByIdAsync(Guid id)
        {
            var wishlist = await _context.Wishlists.Include(w => w.WishlistItems)
        .ThenInclude(wi => wi.Product).FirstOrDefaultAsync(wl => wl.Id == id);
            return wishlist;
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistByUserAsync(Guid userId)
        {
            return await _context.Wishlists.Include("WishlistItems").Include("Product").Where(wl => wl.UserId == userId).ToListAsync();
        }

        public async Task<Wishlist> UpdateWishlistByIdAsync(Wishlist wishlist)
        {
            _context.Wishlists.Update(wishlist);
            await _context.SaveChangesAsync();
            return wishlist;
        }
    }
}