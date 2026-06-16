using SportShop.Data;
using SportShop.Models.Interfaces;

namespace SportShop.Models.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly SportShopDbContext _context;
        public ProductRepository(SportShopDbContext context) => _context = context;

        public IEnumerable<Product> GetAllProducts() => _context.Products;
        public IEnumerable<Product> GetTrendingProducts() => _context.Products.Where(p => p.IsTrendingProduct);
        public Product? GetProductDetail(int id) => _context.Products.FirstOrDefault(p => p.Id == id);
    }
}