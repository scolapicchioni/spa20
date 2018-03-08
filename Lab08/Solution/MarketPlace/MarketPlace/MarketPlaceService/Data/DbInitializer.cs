using MarketPlaceService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceService.Data
{
    public static class DbInitializer {
        public static void Initialize(MarketPlaceContext context, string path) {
            //using Microsoft.EntityFrameworkCore;
            context.Database.Migrate();

            // Look for any products.
            if (context.Products.Any()) {
                return;   // DB has been seeded
            }

            //using MarketPlaceService.Models;
            var products = new Product[] {
                new Product { Name = "Product 1", Description = "First Sample Product", Price = 1234 , UserName = "alice@gmail.com",
                    ImageFile = File.ReadAllBytes($@"{path}\Images\flower.jpg"), ImageMimeType = "image/jpeg"},
                new Product { Name = "Product 2", Description = "Lorem Ipsum", Price = 555 , UserName = "bob@gmail.com",
                    ImageFile = File.ReadAllBytes($@"{path}\Images\orchard.jpg"), ImageMimeType = "image/jpeg"},
                new Product { Name = "Product 3", Description = "Third Sample Product", Price = 333 , UserName = "alice@alice.com",
                    ImageFile = File.ReadAllBytes($@"{path}\Images\path.jpg"), ImageMimeType = "image/jpeg"},
                new Product { Name = "Product 4", Description = "Fourth Sample Product", Price = 44 , UserName = "alice@gmail.com",
                    ImageFile = File.ReadAllBytes($@"{path}\Images\blackberries.jpg"), ImageMimeType = "image/jpeg"}
            };

            foreach (var product in products) {
                context.Products.Add(product);
            }

            context.SaveChanges();
        }
    }
}
