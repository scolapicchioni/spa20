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
