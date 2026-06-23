using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportShop.Models;
using SportShop.Models.Interfaces;
using System;
using System.Linq;

namespace SportShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _cartRepository;

        public OrdersController(IOrderRepository orderRepository, IShoppingCartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        // 1. Ép khách phải đăng nhập mới cho mở trang điền địa chỉ mua hàng
        [Authorize]
        public IActionResult Checkout() => View();

        // 2. Xem lịch sử đơn hàng của tôi (Bắt buộc Đăng nhập)
        [Authorize]
        public IActionResult MyOrders()
        {
            string userEmail = User.Identity?.Name ?? "";
            var myOrders = _orderRepository.GetOrdersByEmail(userEmail);
            return View(myOrders);
        }

        // 3. Xử lý lưu đơn hàng và gán Email tài khoản tự động
        [Authorize]
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            var items = _cartRepository.GetAllShoppingCartItems();
            if (items.Count == 0)
            {
                ModelState.AddModelError("", "Giỏ hàng của bạn đang trống!");
                return View(order);
            }

            // 🔥 DÒNG QUAN TRỌNG NHẤT: Ép Email của đơn hàng bằng chính Email tài khoản đang đăng nhập
            order.Email = User.Identity?.Name;

            // Tự động gán trạng thái đơn hàng dựa trên phương thức khách chọn (COD, MoMo, VNPAY)
            if (order.PaymentMethod == "COD")
            {
                order.Status = "Chờ giao hàng (COD)";
            }
            else
            {
                order.Status = "Đã thanh toán  " + order.PaymentMethod + "";
            }

            // Lưu đơn hàng vào SQL Server thông qua Repository
            _orderRepository.PlaceOrder(order);

            // Xóa sạch giỏ hàng hiện tại
            _cartRepository.ClearCart();
            HttpContext.Session.SetInt32("CartCount", 0);

            // Chuyển sang trang hoàn tất và ném theo phương thức thanh toán để vẽ mã QR giả lập
            return RedirectToAction("CheckoutComplete", new { method = order.PaymentMethod });
        }

        // 4. Nhận tham số phương thức thanh toán truyền sang để hiển thị cho View chúc mừng
        public IActionResult CheckoutComplete(string method)
        {
            ViewBag.PaymentMethod = method;
            return View();
        }
    }
}