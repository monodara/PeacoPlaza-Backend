using Server.Core.src.Entity;

namespace Server.Service.src.DTO
{
    public class WishlistItemReadDto
    {
        public ProductReadDto Product { get; set; }
        public WishlistItemReadDto Transform(WishlistItem wishlistItem){
            Product = new ProductReadDto().Transform(wishlistItem.Product);
            return this;
        }
    }
}