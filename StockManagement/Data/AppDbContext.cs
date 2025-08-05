using Microsoft.EntityFrameworkCore;
using StockManagement.Models;
using System;

namespace StockManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure table names
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
            modelBuilder.Entity<User>().ToTable("Users");

            // Configure composite key for OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ProductId });

            // Configure Order-OrderItem relationship
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Product-OrderItem relationship
            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure User-Order relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed initial users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin123",
                    Role = "Manager"
                },
                new User
                {
                    Id = 2,
                    Username = "user1",
                    Password = "user123",
                    Role = "User"
                },
                new User
                {
                    Id = 3,
                    Username = "manager2",
                    Password = "mgr12345",
                    Role = "Manager"
                }
            );

            // Seed initial products with standardized image paths
            var seedDate = DateTime.Now.AddDays(-10);
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "ASUS TUF Gaming A15 (2024)",
                    Price = 1299.99m,
                    Quantity = 15,
                    Description = "15.6\" FHD 144Hz, AMD Ryzen 7, 16GB DDR5, 1TB SSD, GeForce RTX 4060",
                    PurchaseCount = 42,
                    LastPurchasedDate = seedDate.AddDays(9),
                    ImageUrl = "/images/asus_tuf_gaming.png"
                },
                new Product
                {
                    Id = 2,
                    Name = "Amazon Kindle Paperwhite Signature",
                    Price = 189.99m,
                    Quantity = 25,
                    Description = "32 GB, 6.8\" display, wireless charging, auto-adjusting front light",
                    PurchaseCount = 38,
                    LastPurchasedDate = seedDate.AddDays(7),
                    ImageUrl = "/images/amazon_kindle.png"
                },
                new Product
                {
                    Id = 3,
                    Name = "Amazon Fire TV Stick 4K",
                    Price = 49.99m,
                    Quantity = 50,
                    Description = "Streaming device with 4K Ultra HD, Dolby Vision, and Alexa Voice Remote",
                    PurchaseCount = 65,
                    LastPurchasedDate = seedDate.AddDays(8),
                    ImageUrl = "/images/fire_tv_4k.png"
                },
                new Product
                {
                    Id = 4,
                    Name = "Amazon Fire TV Stick HD",
                    Price = 39.99m,
                    Quantity = 45,
                    Description = "HD streaming device with Alexa Voice Remote",
                    PurchaseCount = 52,
                    LastPurchasedDate = seedDate.AddDays(6),
                    ImageUrl = "/images/fire_tv_hd.png"
                },
                new Product
                {
                    Id = 5,
                    Name = "Amazon Fire TV Stick 4K Max",
                    Price = 59.99m,
                    Quantity = 35,
                    Description = "Our most powerful streaming stick with Wi-Fi 6 support",
                    PurchaseCount = 48,
                    LastPurchasedDate = seedDate.AddDays(7),
                    ImageUrl = "/images/fire_tv_4k_max.png"
                },
                new Product
                {
                    Id = 6,
                    Name = "Philips 3300 Series Espresso Machine",
                    Price = 699.99m,
                    Quantity = 12,
                    Description = "With milk frother, 12 coffee varieties, intuitive touch display",
                    PurchaseCount = 15,
                    LastPurchasedDate = seedDate.AddDays(5),
                    ImageUrl = "/images/philips_3300.png"
                },
                new Product
                {
                    Id = 7,
                    Name = "Samsung Galaxy S25 Ultra",
                    Price = 1199.99m,
                    Quantity = 30,
                    Description = "Titanium Black, 512GB, AI Camera System",
                    PurchaseCount = 28,
                    LastPurchasedDate = seedDate.AddDays(8),
                    ImageUrl = "/images/galaxy_s25.png"
                },
                new Product
                {
                    Id = 8,
                    Name = "HP Envy Inspire 7958e",
                    Price = 199.99m,
                    Quantity = 18,
                    Description = "All-in-One Printer with wireless connectivity",
                    PurchaseCount = 22,
                    LastPurchasedDate = seedDate.AddDays(6),
                    ImageUrl = "/images/hp_envy_printer.png"
                },
                new Product
                {
                    Id = 9,
                    Name = "Breville Barista Express",
                    Price = 599.99m,
                    Quantity = 10,
                    Description = "Espresso Machine with built-in grinder",
                    PurchaseCount = 18,
                    LastPurchasedDate = seedDate.AddDays(4),
                    ImageUrl = "/images/breville_barista.png"
                },
                new Product
                {
                    Id = 10,
                    Name = "BISSELL Little Green Cleaner",
                    Price = 123.99m,
                    Quantity = 20,
                    Description = "Portable deep cleaner for carpets and upholstery",
                    PurchaseCount = 25,
                    LastPurchasedDate = seedDate.AddDays(7),
                    ImageUrl = "/images/bissell_cleaner.png"
                }
            );

            // Seed sample orders
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    UserId = 2,
                    OrderDate = DateTime.Now.AddDays(-2),
                    Status = "Pending"
                },
                new Order
                {
                    Id = 2,
                    UserId = 2,
                    OrderDate = DateTime.Now.AddDays(-1),
                    Status = "Processing"
                }
            );

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    OrderId = 1,
                    ProductId = 3,
                    Quantity = 2,
                    UnitPrice = 49.99m
                },
                new OrderItem
                {
                    OrderId = 1,
                    ProductId = 4,
                    Quantity = 1,
                    UnitPrice = 39.99m
                },
                new OrderItem
                {
                    OrderId = 2,
                    ProductId = 7,
                    Quantity = 1,
                    UnitPrice = 1199.99m
                }
            );
        }
    }
}