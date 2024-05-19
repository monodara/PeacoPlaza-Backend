using Server.Core.src.Entity;

namespace Server.Service.src.DTO
{
    public class WishlistReadDto
    {
        public Guid UserId{ get; set; }
        public string Name { get; set; }
        public IEnumerable<WishlistItemReadDto> WishlistItems { get; set; }
        public WishlistReadDto Transform(Wishlist wishlist)
        {
            Name = wishlist.Name;
            UserId = wishlist.UserId;
            WishlistItems = wishlist.WishlistItems.Select(item => new WishlistItemReadDto().Transform(item)).ToList();
            return this;
        }
    }

    public class WishlistCreateDto
    {
        public string Name { get; set; }
        public WishlistCreateDto(string name)
        {
            Name = name;
        }
        public Wishlist CreateWishlist(Guid userId)
        {
            return new Wishlist(Name, userId);
        }
    }
    public class WishlistUpdateDto
    {
        public string Name { get; set; }
        public WishlistUpdateDto(string name)
        {
            Name = name;
        }

        public Wishlist UpdateWishlist(Wishlist oldWishlist)
        {
            oldWishlist.Name = Name;
            return oldWishlist;
        }
    }
}