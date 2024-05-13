using Server.Core.src.Entity;

namespace Server.Service.src.DTO
{
    public class WishlistReadDto
    {
        public string Name { get; set; }
        public WishlistReadDto Transform(Wishlist wishlist)
        {
            Name = wishlist.Name;
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
        public Guid Id { get; set; }
        public string Name { get; set; }
        public WishlistUpdateDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Wishlist UpdateWishlist(Wishlist oldWishlist)
        {
            oldWishlist.Name = Name;
            return oldWishlist;
        }
    }
}