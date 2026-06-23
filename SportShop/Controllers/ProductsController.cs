using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportShop.Models;
using SportShop.Models.Interfaces;

namespace SportShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository) => _productRepository = productRepository;

        public IActionResult Shop(string category)
        {
            var products = _productRepository.GetAllProducts();
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category);
                ViewBag.CurrentCategory = category;
            }
            return View(products);
        }

        public IActionResult Detail(int id)
        {
            var product = _productRepository.GetProductDetail(id);
            if (product == null) return NotFound();

            // Lấy danh sách bình luận của sản phẩm này
            var reviews = _productRepository.GetReviewsForProduct(id);
            ViewBag.Reviews = reviews;

            // Tính điểm sao trung bình (Ví dụ: 4.5 sao)
            ViewBag.AverageRating = reviews.Any() ? Math.Round(reviews.Average(r => r.Rating), 1) : 0;

            return View(product);
        }

        // Hàm xử lý khi khách bấm gửi Đánh giá (Bắt buộc Đăng nhập)
        [Authorize]
        [HttpPost]
        public IActionResult AddReview(int productId, int rating, string comment)
        {
            if (rating >= 1 && rating <= 5)
            {
                var review = new ProductReview
                {
                    ProductId = productId,
                    Rating = rating,
                    Comment = comment,
                    UserEmail = User.Identity?.Name, // Lấy email người đang đăng nhập
                    DateReviewed = DateTime.Now
                };
                _productRepository.AddReview(review);
            }
            return RedirectToAction("Detail", new { id = productId });
        }
    }
}