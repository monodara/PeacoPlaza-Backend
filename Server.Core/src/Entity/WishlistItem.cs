namespace Server.Core.src.Entity
{
    public class WishlistItem : BaseEntity
    {
        // foreign key
        public Guid ProductId { get; set; }
        // navigation
        public Product Product { get; set; }
        //foreign key
        public Guid WishlistId { get; set; }
        // navigation
        public Wishlist Wishlist { get; set; }
        public WishlistItem(Guid productId, Guid wishlistId)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            WishlistId = wishlistId;
        }
    }
}