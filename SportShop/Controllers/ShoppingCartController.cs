using Microsoft.AspNetCore.Mvc;
using SportShop.Models.Interfaces;

namespace SportShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public ShoppingCartController(IShoppingCartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var items = _cartRepository.GetAllShoppingCartItems();
            ViewBag.TotalCart = _cartRepository.GetShoppingCartTotal();
            return View(items);
        }

        // 1. Sửa kiểu trả về thành IActionResult ở đây
        public IActionResult AddToShoppingCart(int pId, string returnUrl)
        {
            var product = _productRepository.GetAllProducts().FirstOrDefault(p => p.Id == pId);
            if (product != null)
            {
                _cartRepository.AddToCart(product);
            }

            // Cập nhật bộ đếm badge menu
            int count = _cartRepository.GetAllShoppingCartItems().Sum(i => i.Qty);
            HttpContext.Session.SetInt32("CartCount", count);

            // Có IActionResult, cả 2 lệnh return dưới đây đều chạy mượt mà không lỗi
            if (!string.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index");
        }

        // 2. Sửa tương tự cho hàm xóa sản phẩm thành IActionResult
        public IActionResult RemoveFromShoppingCart(int pId)
        {
            var product = _productRepository.GetAllProducts().FirstOrDefault(p => p.Id == pId);
            if (product != null)
            {
                _cartRepository.RemoveFromCart(product);
            }

            int count = _cartRepository.GetAllShoppingCartItems().Sum(i => i.Qty);
            HttpContext.Session.SetInt32("CartCount", count);

            return RedirectToAction("Index");
        }
    }
}