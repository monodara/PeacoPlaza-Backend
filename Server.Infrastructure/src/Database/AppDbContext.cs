using Microsoft.EntityFrameworkCore;
using Server.Core.src.Entity;
using Server.Core.src.ValueObject;

namespace Server.Infrastructure.src.Database;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } // table `Users` -> `users`
    public DbSet<Address> Addresses { get; set; } // table `Addresses` -> `addresses`
    public DbSet<Wishlist> Wishlists { get; set; } // table `Wishlists` -> `wishlists`
    public DbSet<WishlistItem> WishlistItems { get; set; }
    public DbSet<Order> Orders { get; set; } // table `Orders` -> `orders`
    public DbSet<OrderProduct> OrderProducts { get; set; } // table `Orders` -> `orders`
    public DbSet<Review> Reviews { get; set; } // table `Reviews` -> `reviews`
    public DbSet<ReviewImage> ReviewImages { get; set; } // table `Reviews` -> `reviews`
    public DbSet<Payment> Payments { get; set; } // table `Payment` -> `payment`
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    static AppDbContext()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Role>();
        modelBuilder.HasPostgresEnum<PaymentMethod>();
        modelBuilder.HasPostgresEnum<Status>();

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasData(SeedingData.Users);
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<Category>(e =>
            {
                e.HasData(SeedingData.GetCategories());
                e.HasIndex(e => e.Name).IsUnique();
            });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<Product>(e =>
        {
            e.HasData(SeedingData.Products);
            e.HasIndex(p => p.Name).IsUnique();
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<ProductImage>(e =>
        {
            e.HasData(SeedingData.GetProductImages());
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<ReviewImage>(entity =>
        {
            entity.HasNoKey();
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasIndex(wl => new { wl.Name, wl.UserId }).IsUnique();
            entity.HasData(SeedingData.Wishlists);
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<WishlistItem>(entity =>
        {
            entity.HasData(SeedingData.GetWishlistItems());
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasData(SeedingData.Addresses);
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasData(SeedingData.Orders);
        });
        // -----------------------------------------------------------------------------------------------
        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasData(SeedingData.OrderProducts);
        });

        base.OnModelCreating(modelBuilder);
    }
}
