using MarketPlaceService.Models;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlaceService.Data {
    public class ProductsRepository : IProductsRepository {
        private MarketPlaceContext _context;
        public ProductsRepository(MarketPlaceContext context) {
            _context = context;
            if (_context.Products.Count()==0)
                Add(new Product { Name = "Product 1", Description = "First Sample Product", Price = 1234 });
        }

        public void Add(Product product) {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public Product Find(int id) {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetAll() {
            return _context.Products.ToList();
        }

        public Product Remove(int id) {
            var entity = _context.Products.First(p => p.Id == id);
            _context.Products.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public void Update(Product product) {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}
