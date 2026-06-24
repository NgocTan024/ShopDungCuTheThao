using Microsoft.AspNetCore.Mvc;
using SportShop.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository) => _productRepository = productRepository;

        // Hàm Shop tích hợp nhận tham số Search, Sort và Số trang
        public IActionResult Shop(string category, string searchString, string sortOrder, int pageNumber = 1)
        {
            // 1. Gửi ngược các bộ lọc ra View để giữ trạng thái thanh tìm kiếm/sắp xếp
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentSort = sortOrder;

            // Lấy 20 sản phẩm gốc từ database lên
            var products = _productRepository.GetAllProducts();

            // 2. XỬ LÝ TÌM KIẾM (SEARCH BAR)
            if (!string.IsNullOrEmpty(searchString))
            {
                // Tìm sản phẩm có tên hoặc mô tả chứa từ khóa khách gõ
                products = products.Where(p => p.Name!.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                            || p.Detail!.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            // 3. XỬ LÝ LỌC THEO DANH MỤC (SIDEBAR)
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category);
            }

            // 4. XỬ LÝ SẮP XẾP GIÁ (SORTING DROPDOWN)
            switch (sortOrder)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price); // Thấp đến cao
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price); // Cao đến thấp
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name); // Z - A
                    break;
                default:
                    products = products.OrderBy(p => p.Name); // Mặc định A - Z
                    break;
            }

            // 5. THUẬT TOÁN PHÂN TRANG (PAGINATION)
            int pageSize = 6; // Ép mỗi trang chỉ hiện tối đa 6 sản phẩm cho cân đối
            int totalItems = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (pageNumber < 1) pageNumber = 1;
            if (pageNumber > totalPages && totalPages > 0) pageNumber = totalPages;

            // Lệnh Skip/Take để cắt dữ liệu theo trang hiện tại
            var paginatedProducts = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Đẩy các thông số phân trang ra giao diện để vẽ nút bấm 1, 2, 3
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPreviousPage = pageNumber > 1;
            ViewBag.HasNextPage = pageNumber < totalPages;

            return View(paginatedProducts);
        }

        // Trang xem chi tiết sản phẩm
        public IActionResult Detail(int id)
        {
            var product = _productRepository.GetProductDetail(id);
            if (product == null) return NotFound();

            var reviews = _productRepository.GetReviewsForProduct(id);
            ViewBag.Reviews = reviews;
            ViewBag.AverageRating = reviews.Any() ? Math.Round(reviews.Average(r => r.Rating), 1) : 0;

            return View(product);
        }

        // 🔥 API CHẠY NGẦM: Lấy danh sách 5 sản phẩm gợi ý nhanh dạng JSON khi khách đang gõ chữ
        [HttpGet]
        public IActionResult GetSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term) || term.Length < 1)
            {
                return Json(new List<object>());
            }

            // Lọc ra tối đa 5 sản phẩm khớp tên hoặc phân loại để hiển thị popup gợi ý nhanh
            var suggestions = _productRepository.GetAllProducts()
                .Where(p => p.Name!.Contains(term, StringComparison.OrdinalIgnoreCase)
                         || p.Category!.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Take(5)
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    price = p.Price.ToString("N0") + " đ",
                    imageUrl = p.ImageUrl,
                    category = p.Category
                })
                .ToList();

            return Json(suggestions);
        }
    }
}