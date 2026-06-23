using SportShop.Models;

namespace SportShop.Models.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetTrendingProducts();
        Product? GetProductDetail(int id);
        void AddReview(ProductReview review);
        IEnumerable<ProductReview> GetReviewsForProduct(int productId);
    }
}