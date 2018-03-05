using MarketPlaceService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceService.Data
{
    public interface IProductsRepository {
        Task AddAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> FindAsync(int id);
        Task<Product> RemoveAsync(int id);
        Task UpdateAsync(Product product);
    }
}
