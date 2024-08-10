using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaparaDigitalProductPlatform.Domain.Entities;
using System;
using System.Linq;
using PaparaDigitalProductPlatform.Persistance;

namespace PaparaDigitalProductPlatform.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category { Name = "Windows Licenses", Url = "windows-licenses", Tags = "windows,licenses" },
                        new Category { Name = "Office Licenses", Url = "office-licenses", Tags = "office,licenses" },
                        new Category { Name = "Other Software", Url = "other-software", Tags = "software,licenses" },
                        new Category { Name = "Antivirus Software", Url = "antivirus-software", Tags = "antivirus,software" }
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com", Password = "password", Role = "User", Points = 20 },
                        new User { FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", Password = "password", Role = "User",  Points = 15 },
                        new User { FirstName = "Charlie", LastName = "Brown", Email = "charlie.brown@example.com", Password = "password", Role = "User", Points = 30 },
                        new User { FirstName = "David", LastName = "Wilson", Email = "david.wilson@example.com", Password = "password", Role = "User",  Points = 25 },
                        new User { FirstName = "Eve", LastName = "Davis", Email = "eve.davis@example.com", Password = "password", Role = "User", Points = 10 }
                    );
                    context.SaveChanges();
                }

                if (!context.Products.Any())
                {
                    var windowsCategoryId = context.Categories.First(c => c.Name == "Windows Licenses").Id;
                    var officeCategoryId = context.Categories.First(c => c.Name == "Office Licenses").Id;
                    var softwareCategoryId = context.Categories.First(c => c.Name == "Other Software").Id;
                    var antivirusCategoryId = context.Categories.First(c => c.Name == "Antivirus Software").Id;

                    context.Products.AddRange(
                        // Windows Licenses
                        new Product { Name = "Windows 10 Home", Description = "Genuine Windows 10 Home License", Price = 99.99m, IsActive = true, PointRate = 0.1m, MaxPoint = 10, Stock = 100,  CategoryId = windowsCategoryId },
                        new Product { Name = "Windows 10 Pro", Description = "Genuine Windows 10 Pro License", Price = 199.99m, IsActive = true, PointRate = 0.2m, MaxPoint = 20, Stock = 50,  CategoryId = windowsCategoryId },
                        new Product { Name = "Windows 11 Home", Description = "Genuine Windows 11 Home License", Price = 109.99m, IsActive = true, PointRate = 0.1m, MaxPoint = 10, Stock = 120,  CategoryId = windowsCategoryId },
                        new Product { Name = "Windows 11 Pro", Description = "Genuine Windows 11 Pro License", Price = 219.99m, IsActive = true, PointRate = 0.2m, MaxPoint = 20, Stock = 80,  CategoryId = windowsCategoryId },

                        // Office Licenses
                        new Product { Name = "Microsoft Office 2019 Home & Student", Description = "Genuine Microsoft Office 2019 Home & Student License", Price = 149.99m, IsActive = true, PointRate = 0.15m, MaxPoint = 15, Stock = 100,  CategoryId = officeCategoryId },
                        new Product { Name = "Microsoft Office 2019 Home & Business", Description = "Genuine Microsoft Office 2019 Home & Business License", Price = 249.99m, IsActive = true, PointRate = 0.2m, MaxPoint = 20, Stock = 70,  CategoryId = officeCategoryId },
                        new Product { Name = "Microsoft Office 2021 Home & Student", Description = "Genuine Microsoft Office 2021 Home & Student License", Price = 159.99m, IsActive = true, PointRate = 0.15m, MaxPoint = 15, Stock = 100,  CategoryId = officeCategoryId },
                        new Product { Name = "Microsoft Office 2021 Home & Business", Description = "Genuine Microsoft Office 2021 Home & Business License", Price = 269.99m, IsActive = true, PointRate = 0.2m, MaxPoint = 20, Stock = 60,  CategoryId = officeCategoryId },

                        // Other Software
                        new Product { Name = "Adobe Photoshop Elements", Description = "Genuine Adobe Photoshop Elements License", Price = 99.99m, IsActive = true, PointRate = 0.1m, MaxPoint = 10, Stock = 200,  CategoryId = softwareCategoryId },
                        new Product { Name = "CorelDRAW Graphics Suite", Description = "Genuine CorelDRAW Graphics Suite License", Price = 399.99m, IsActive = true, PointRate = 0.25m, MaxPoint = 40, Stock = 50,  CategoryId = softwareCategoryId },

                        // Antivirus Software
                        new Product { Name = "Norton Antivirus", Description = "Genuine Norton Antivirus License", Price = 39.99m, IsActive = true, PointRate = 0.05m, MaxPoint = 5, Stock = 150,  CategoryId = antivirusCategoryId },
                        new Product { Name = "McAfee Total Protection", Description = "Genuine McAfee Total Protection License", Price = 49.99m, IsActive = true, PointRate = 0.05m, MaxPoint = 5, Stock = 200,  CategoryId = antivirusCategoryId },
                        new Product { Name = "Kaspersky Internet Security", Description = "Genuine Kaspersky Internet Security License", Price = 59.99m, IsActive = true, PointRate = 0.1m, MaxPoint = 6, Stock = 120,  CategoryId = antivirusCategoryId },
                        new Product { Name = "Bitdefender Total Security", Description = "Genuine Bitdefender Total Security License", Price = 69.99m, IsActive = true, PointRate = 0.1m, MaxPoint = 7, Stock = 100,  CategoryId = antivirusCategoryId }
                    );
                    context.SaveChanges();
                }

                if (!context.Coupons.Any())
                {
                    context.Coupons.AddRange(
                        new Coupon { Code = "WIN10OFF", Amount = 10, ExpiryDate = DateTime.UtcNow.AddMonths(1), IsActive = true, UsageCount = 0 },
                        new Coupon { Code = "WIN11OFF", Amount = 15, ExpiryDate = DateTime.UtcNow.AddMonths(1), IsActive = true, UsageCount = 0 },
                        new Coupon { Code = "OFFICE19OFF", Amount = 20, ExpiryDate = DateTime.UtcNow.AddMonths(1), IsActive = true, UsageCount = 0 },
                        new Coupon { Code = "OFFICE21OFF", Amount = 25, ExpiryDate = DateTime.UtcNow.AddMonths(1), IsActive = true, UsageCount = 0 },
                        new Coupon { Code = "ADOBE10OFF", Amount = 10, ExpiryDate = DateTime.UtcNow.AddMonths(1), IsActive = true, UsageCount = 0 },
                        new Coupon { Code = "COREL15OFF", Amount = 15, ExpiryDate = DateTime.UtcNow.AddMonths(1), IsActive = true, UsageCount = 0 },
                        new Coupon { Code = "ANTIVIRUS5OFF", Amount = 5, ExpiryDate = DateTime.UtcNow.AddMonths(1), IsActive = true, UsageCount = 0 }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
