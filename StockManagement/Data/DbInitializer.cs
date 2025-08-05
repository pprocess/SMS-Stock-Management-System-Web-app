using StockManagement.Models;
using System;
using System.Linq;

namespace StockManagement.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any() || context.Products.Any())
            {
                return;
            }

            var users = new User[]
            {
                new User { Username = "admin", Password = "admin123", Role = "Manager" },
                new User { Username = "user1", Password = "user123", Role = "User" },
                new User { Username = "user2", Password = "user123", Role = "User" }
            };
            context.Users.AddRange(users);

            // Seed Products
            var products = new Product[]
            {
                new Product
                {
                    Name = "ASUS TUF Gaming A15 (2024)",
                    Price = 1299.99m,
                    Quantity = 15,
                    Description = "15.6\" FHD 144Hz, AMD Ryzen 7, 16GB DDR5, 1TB SSD, GeForce RTX 4060",
                    PurchaseCount = 42,
                    LastPurchasedDate = DateTime.Now.AddDays(-1),
                    ImageUrl = "images/ausus tuff.png"  // Updated path
                },
                new Product
                {
                    Name = "Amazon Kindle Paperwhite Signature",
                    Price = 189.99m,
                    Quantity = 25,
                    Description = "32 GB, 6.8\" display, wireless charging, auto-adjusting front light",
                    PurchaseCount = 38,
                    LastPurchasedDate = DateTime.Now.AddDays(-3),
                    ImageUrl = "images/amazon kindle.png"  // Updated path
                },
                new Product
                {
                    Name = "Amazon Fire TV Stick 4K",
                    Price = 49.99m,
                    Quantity = 50,
                    Description = "Streaming device with 4K Ultra HD, Dolby Vision, and Alexa Voice Remote",
                    PurchaseCount = 65,
                    LastPurchasedDate = DateTime.Now.AddDays(-2),
                    ImageUrl = "images/tv stick 4k.png"  // Updated path
                },
                new Product
                {
                    Name = "Amazon Fire TV Stick HD",
                    Price = 39.99m,
                    Quantity = 45,
                    Description = "HD streaming device with Alexa Voice Remote",
                    PurchaseCount = 52,
                    LastPurchasedDate = DateTime.Now.AddDays(-4),
                    ImageUrl = "images/tv stick hd.png"  // Updated path
                },
                new Product
                {
                    Name = "Amazon Fire TV Stick 4K Max",
                    Price = 59.99m,
                    Quantity = 35,
                    Description = "Our most powerful streaming stick with Wi-Fi 6 support",
                    PurchaseCount = 48,
                    LastPurchasedDate = DateTime.Now.AddDays(-3),
                    ImageUrl = "images/tv stick 4k max.png"  // Updated path
                },
                new Product
                {
                    Name = "Philips 3300 Series Espresso Machine",
                    Price = 699.99m,
                    Quantity = 12,
                    Description = "With milk frother, 12 coffee varieties, intuitive touch display",
                    PurchaseCount = 15,
                    LastPurchasedDate = DateTime.Now.AddDays(-5),
                    ImageUrl = "images/philips 3300.png"  // Updated path
                },
                new Product
                {
                    Name = "Samsung Galaxy S25 Ultra",
                    Price = 1199.99m,
                    Quantity = 30,
                    Description = "Titanium Black, 512GB, AI Camera System",
                    PurchaseCount = 28,
                    LastPurchasedDate = DateTime.Now.AddDays(-2),
                    ImageUrl = "images/galaxy s25.png"  // Updated path
                },
                new Product
                {
                    Name = "HP Envy Inspire 7958e",
                    Price = 199.99m,
                    Quantity = 18,
                    Description = "All-in-One Printer with wireless connectivity",
                    PurchaseCount = 22,
                    LastPurchasedDate = DateTime.Now.AddDays(-4),
                    ImageUrl = "images/envy printer.png"  // Updated path
                },
                new Product
                {
                    Name = "Breville Barista Express",
                    Price = 599.99m,
                    Quantity = 10,
                    Description = "Espresso Machine with built-in grinder",
                    PurchaseCount = 18,
                    LastPurchasedDate = DateTime.Now.AddDays(-6),
                    ImageUrl = "images/breville barista.png"  // Updated path
                },
                new Product
                {
                    Name = "BISSELL Little Green Cleaner",
                    Price = 123.99m,
                    Quantity = 20,
                    Description = "Portable deep cleaner for carpets and upholstery",
                    PurchaseCount = 25,
                    LastPurchasedDate = DateTime.Now.AddDays(-3),
                    ImageUrl = "images/bissell carpet cleaner.png"  // Updated path
                }
            };
            context.Products.AddRange(products);

            context.SaveChanges();
        }
    }
}