using MarketPlaceService.Models;
using System.Collections.Generic;

namespace MarketPlaceService.Data {
    public interface IProductsRepository {
        void Add(Product product);
        IEnumerable<Product> GetAll();
        Product Find(int id);
        Product Remove(int id);
        void Update(Product product);
    }
}
