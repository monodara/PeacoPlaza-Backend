using System.ComponentModel.DataAnnotations;

namespace Server.Core.src.Entity
{
    public class Wishlist : BaseEntity
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        // foreign key
        public Guid UserId { get; private set; }
        public User User { get; set; }

        // relation - a wishlist contains a list of wishlish items
        public IEnumerable<WishlistItem> WishlistItems { get; set; }


        public Wishlist(string name, Guid userId)
        {
            Id = Guid.NewGuid();
            Name = name;
            UserId = userId;
        }
    }
}