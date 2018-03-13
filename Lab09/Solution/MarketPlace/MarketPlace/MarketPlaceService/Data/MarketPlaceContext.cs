using MarketPlaceService.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceService.Data {
    public class MarketPlaceContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public MarketPlaceContext(DbContextOptions<MarketPlaceContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
        }
    }
}
