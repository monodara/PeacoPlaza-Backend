// using Server.Core.src.Entity;
// using Server.Core.src.ValueObject;
// using Server.Service.src.ServiceAbstract.AuthServiceAbstract;
// using Server.Service.src.Shared;
// using System;

// namespace Server.Infrastructure.src.Database;

// public class SeedingData
// {
//     private static Random random = new Random();
//     private static IPasswordService pwdService = new PasswordService();

//     private static Category category1 = new Category("Electronics", $"https://picsum.photos/200/?random={random.Next(10)}");
//     private static Category category2 = new Category("Clothing", $"https://picsum.photos/200/?random={random.Next(10)}");
//     private static Category category3 = new Category("Home and Furnitures", $"https://picsum.photos/200/?random={random.Next(10)}");
//     private static Category category4 = new Category("Books", $"https://picsum.photos/200/?random={random.Next(10)}");
//     private static Category category5 = new Category("Toys and Games", $"https://picsum.photos/200/?random={random.Next(10)}");
//     private static Category category6 = new Category("Sports", $"https://picsum.photos/200/?random={random.Next(10)}");

//     public static List<Category> GetCategories()
//     {
//         return new List<Category>
//         {
//             category1, category2, category3, category4, category5, category6
//         };
//     }

//     private static List<Product> GenerateProductsForCategory(Category category, int count)
//     {
//         var products = new List<Product>();
//         for (int i = 1; i <= count; i++)
//         {
//             var product = new Product(
//                 $"{category.Name} product {i}",
//                 random.Next(1000),      // price
//                 $"Description of {category.Name} product {i}",
//                 1000000,        // inventory
//                 random.Next(1, 10) / 10M,               // weight
//                 category.Id
//             );
//             products.Add(product);
//         }
//         return products;
//     }

//     public static List<Product> GetProducts()
//     {
//         var products = new List<Product>();
//         products.AddRange(GenerateProductsForCategory(category1, 10));
//         products.AddRange(GenerateProductsForCategory(category2, 10));
//         products.AddRange(GenerateProductsForCategory(category3, 10));
//         products.AddRange(GenerateProductsForCategory(category4, 10));
//         products.AddRange(GenerateProductsForCategory(category5, 10));
//         products.AddRange(GenerateProductsForCategory(category6, 10));

//         return products;
//     }

//     public static List<Product> Products = GetProducts();

//     public static List<ProductImage> GetProductImages()
//     {
//         var paths = new List<string> { "src/Images/fi1.jpeg", "src/Images/fi2.jpeg" };
//         var data = new List<byte[]> { };
//         var images = new List<ProductImage> { };
//         var products = Products;
//         foreach (var path in paths)
//         {
//             try
//             {
//                 var imageData = File.ReadAllBytes(path);
//                 data.Add(imageData);
//             }
//             catch (Exception e)
//             {
//             }
//         }

//         foreach (var product in products)
//         {
//             foreach (var d in data)
//             {
//                 try
//                 {
//                     var image = new ProductImage
//                     {
//                         Id = Guid.NewGuid(),
//                         CreatedAt = DateTime.UtcNow,
//                         UpdatedAt = DateTime.UtcNow,
//                         ProductId = product.Id,
//                         Data = d
//                     };

//                     images.Add(image);
//                 }
//                 catch (Exception ex)
//                 {
//                 }
//             }
//         }
//         return images;
//     }

//     public static List<User> GetUsers()
//     {
//         var users = new List<User>();
//         var user1 = new User("DemoAdmin", "demo.admin@mail.com", "SuperAdmin1234", Role.Admin);
//         user1.Password = pwdService.HashPassword(user1.Password, out byte[] salt1);
//         user1.Salt = salt1;
//         users.Add(user1);
//         var user2 = new User("DemoCustomer", "customer_1@mail.com", "Password1234");
//         user2.Password = pwdService.HashPassword(user2.Password, out byte[] salt2);
//         user2.Salt = salt2;
//         users.Add(user2);

//         for (int i = 3; i <= 10; i++)
//         {
//             var user = new User($"Customer{i}", $"customer_{i}@mail.com", "Password1234");
//             user.Password = pwdService.HashPassword(user.Password, out byte[] salt);
//             user.Salt = salt;
//             users.Add(user);
//         }

//         return users;
//     }
//     public static List<User> Users = GetUsers();

//     public static List<Address> GetAddresses()
//     {
//         var addresses = new List<Address>();
//         var address1 = new Address(
//             "41C", "Asemakatu", "Pori", "Finland", "61200", "4198767000", "John", "Mull", "K-market",
//             Users[1].Id
//         );
//         addresses.Add(address1);
//         for (int i = 1; i < Users.Count; i++)
//         {
//             var address = new Address(
//                 $"Street {i}", $"City {i}", $"State {i}", "Country", $"ZIP{i}", $"Phone{i}", $"FirstName{i}", $"LastName{i}", $"AddressLine{i}",
//                 Users[i].Id
//             );
//             addresses.Add(address);
//         }

//         return addresses;
//     }
//     public static List<Address> Addresses = GetAddresses();

//     public static List<Wishlist> GetWishlists()
//     {
//         var wishlists = new List<Wishlist>();

//         foreach (var user in Users)
//         {
//             var wishlist = new Wishlist($"Example wishlist for {user.UserName}", user.Id);
//             wishlists.Add(wishlist);
//         }

//         return wishlists;
//     }
//     public static List<Wishlist> Wishlists = GetWishlists();

//     public static List<WishlistItem> GetWishlistItems()
//     {
//         var wishlistItems = new List<WishlistItem>();
//         foreach (var wishlist in Wishlists)
//         {
//             wishlistItems.Add(new WishlistItem(Products[random.Next(Products.Count)].Id, wishlist.Id));
//         }
//         return wishlistItems;
//     }

//     public static List<Order> GetOrders()
//     {
//         var orders = new List<Order>();


//         var order1 = new Order { UserId = Users[1].Id, AddressId = Addresses[0].Id };
//         orders.Add(order1);

//         for (int i = 1; i < Users.Count; i++)
//         {
//             var order = new Order { UserId = Users[i].Id, AddressId = Addresses[i].Id };
//             orders.Add(order);
//         }

//         return orders;
//     }
//     public static List<Order> Orders = GetOrders();

//     public static List<OrderProduct> GetOrderProducts()
//     {
//         var orderProducts = new List<OrderProduct>();


//         var orderProduct1 = new OrderProduct { OrderId = Orders[0].Id, ProductId = Products[0].Id, Quantity = 3 };
//         var orderProduct2 = new OrderProduct { OrderId = Orders[0].Id, ProductId = Products[1].Id, Quantity = 1 };
//         orderProducts.Add(orderProduct1);
//         orderProducts.Add(orderProduct2);

//         foreach (var order in Orders)
//         {
//             for (int i = 0; i < 3; i++)
//             {
//                 var orderProduct = new OrderProduct
//                 {
//                     OrderId = order.Id,
//                     ProductId = Products[random.Next(Products.Count)].Id,
//                     Quantity = random.Next(1, 5)
//                 };
//                 orderProducts.Add(orderProduct);
//             }
//         }

//         return orderProducts;
//     }
//     public static List<OrderProduct> OrderProducts = GetOrderProducts();
// }
