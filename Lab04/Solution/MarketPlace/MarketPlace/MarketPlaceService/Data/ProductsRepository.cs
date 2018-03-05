using MarketPlaceService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceService.Data {
    public class ProductsRepository : IProductsRepository {

        private MarketPlaceContext _context;
        public ProductsRepository(MarketPlaceContext context) {
            _context = context;

            //init our db with some products if empty
            if (_context.Products.Count() == 0) {
                _context.Products.AddRange(
                    new Product { Name = "Product 1", Description = "First Sample Product", Price = 1234 },
                    new Product { Name = "Product 2", Description = "Second Sample Product", Price = 2345 },
                    new Product { Name = "Product 3", Description = "Third Sample Product", Price = 3456 },
                    new Product { Name = "Product 4", Description = "Fourth Sample Product", Price = 4567 });
                _context.SaveChanges();
            }
        }

        public async Task AddAsync(Product product) {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> FindAsync(int id) {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync() {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> RemoveAsync(int id) {
            var entity = _context.Products.First(p => p.Id == id);
            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Product product) {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
