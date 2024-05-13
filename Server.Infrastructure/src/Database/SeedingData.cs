using Server.Core.src.Entity;
using Server.Core.src.ValueObject;
using Server.Service.src.ServiceAbstract.AuthServiceAbstract;
using Server.Service.src.Shared;

namespace Server.Infrastructure.src.Database;

public class SeedingData
{
    private static Random random = new Random();
    private static IPasswordService pwdService = new PasswordService();

    private static Category category1 = new Category("Electronics", $"https://picsum.photos/200/?random={random.Next(10)}");
    private static Category category2 = new Category("Clothing", $"https://picsum.photos/200/?random={random.Next(10)}");
    private static Category category3 = new Category("Home and Furnitures", $"https://picsum.photos/200/?random={random.Next(10)}");
    private static Category category4 = new Category("Books", $"https://picsum.photos/200/?random={random.Next(10)}");
    private static Category category5 = new Category("Toys and Games", $"https://picsum.photos/200/?random={random.Next(10)}");
    private static Category category6 = new Category("Sports", $"https://picsum.photos/200/?random={random.Next(10)}");


    public static List<Category> GetCategories()
    {
        return new List<Category>
        {
            category1, category2, category3, category4, category5, category6
        };
    }


    private static List<Product> GenerateProductsForCategory(Category category, int count)
    {
        var products = new List<Product>();
        for (int i = 1; i <= count; i++)
        {
            var product = new Product(
                $"{category.Name} product {i}",
                random.Next(1000),      // price
                $"Description of {category.Name} product {i}",
                random.Next(10),        // inventory
                random.Next(1, 10) / 10M,               // weight
                category.Id
            );
            products.Add(product);
        }
        return products;
    }

    public static List<Product> GetProducts()
    {
        var products = new List<Product>();
        products.AddRange(GenerateProductsForCategory(category1, 20));
        products.AddRange(GenerateProductsForCategory(category2, 20));
        products.AddRange(GenerateProductsForCategory(category3, 20));
        products.AddRange(GenerateProductsForCategory(category4, 20));
        products.AddRange(GenerateProductsForCategory(category5, 20));
        products.AddRange(GenerateProductsForCategory(category6, 20));

        return products;
    }

    public static List<Product> Products = GetProducts();

    public static List<ProductImage> GetProductImages()
    {
        var productImages = new List<ProductImage>();
        foreach (var product in Products)
        {
            for (int i = 0; i < 3; i++)
            {
                var productImage = new ProductImage
                (
                    $"https://picsum.photos/200/?random={random.Next(100, 1000)}",
                    product.Id
                );
                productImages.Add(productImage);
            }
        }
        return productImages;
    }

    public static List<User> GetUsers()
    {
        var users = new List<User>();
        var user1 = new User("DemoAdmin", "demo.admin@mail.com", "SuperAdmin1234", Role.Admin);
        user1.Password = pwdService.HashPassword(user1.Password, out byte[] salt1);
        user1.Salt = salt1;
        users.Add(user1);
        var user2 = new User("DemoCustomer", "customer_1@mail.com", "Password1234");
        user2.Password = pwdService.HashPassword(user2.Password, out byte[] salt2);
        user2.Salt = salt2;
        users.Add(user2);
        return users;
    }
    public static List<User> Users = GetUsers();

    public static List<Address> GetAddresses()
    {
        var user = Users[1];
        var address = new Address(
                "41C", "Asemakatu", "Pori", "Finland", "61200", "4198767000", "John", "Mull", "K-market",
                user.Id
            );
        return [
            address
        ];
    }
    public static List<Address> Addresses = GetAddresses();

    public static List<Wishlist> GetWishlists()
    {
        var wishlists = new List<Wishlist>();

        foreach (var user in Users)
        {
            var wishlist = new Wishlist($"Example wishlist for {user.UserName}", user.Id);
            wishlists.Add(wishlist);
        }

        return wishlists;
    }
    public static List<Wishlist> Wishlists = GetWishlists();

    public static List<WishlistItem> GetWishlistItems()
    {
        var wishlistItem = new WishlistItem(Products[0].Id, Wishlists[0].Id);
        return [
            wishlistItem
        ];
    }
    public static List<Order> GetOrders()
    {
        var order = new Order(Users[1].Id, Addresses[0].Id);
        return [
            order
        ];
    }
    public static List<Order> Orders = GetOrders();
    public static List<OrderProduct> GetOrderProducts()
    {
        var orderProducts = new List<OrderProduct>();
        var orderProduct1 = new OrderProduct(Orders[0].Id, Products[0].Id, 3);
        var orderProduct2 = new OrderProduct(Orders[0].Id, Products[1].Id, 1);
        orderProducts.Add(orderProduct1);
        orderProducts.Add(orderProduct2);
        return orderProducts;
    }
    public static List<OrderProduct> OrderProducts = GetOrderProducts();
}