using Server.Core.src.Entity;

namespace Server.Service.src.DTO
{
    public class WishlistItemReadDto
    {
        public ProductReadDTO Product { get; set; }
        public WishlistItemReadDto Transform(WishlistItem wishlistItem){
            var productReadDto = new ProductReadDTO();
            productReadDto.Transform(wishlistItem.Product);
            Product = productReadDto;
            return this;
        }
    }
}