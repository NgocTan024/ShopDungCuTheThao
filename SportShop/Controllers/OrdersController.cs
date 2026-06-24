using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportShop.Data; // THÊM DÒNG NÀY ĐỂ KẾT NỐI CONTEXT
using SportShop.Models;
using SportShop.Models.Interfaces;
using System;

namespace SportShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _cartRepository;
        private readonly SportShopDbContext _context; // ĐÃ THÊM

        public OrdersController(IOrderRepository orderRepository, IShoppingCartRepository cartRepository, SportShopDbContext context)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _context = context; // ĐÃ THÊM
        }

        [Authorize]
        public IActionResult Checkout() => View();

        [Authorize]
        public IActionResult MyOrders()
        {
            string userEmail = User.Identity?.Name ?? "";
            var myOrders = _orderRepository.GetOrdersByEmail(userEmail);
            return View(myOrders);
        }

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

            order.Email = User.Identity?.Name;
            order.OrderPlaced = DateTime.Now;
            order.OrderTotal = _cartRepository.GetShoppingCartTotal();

            if (order.PaymentMethod == "COD")
            {
                order.Status = "Chờ giao hàng (COD)";
            }
            else
            {
                order.Status = "Chờ chuyển khoản ngân hàng";
            }

            _orderRepository.PlaceOrder(order);

            int newOrderId = order.Id;
            decimal totalAmount = order.OrderTotal;
            string selectedMethod = order.PaymentMethod;

            _cartRepository.ClearCart();
            HttpContext.Session.SetInt32("CartCount", 0);

            return RedirectToAction("CheckoutComplete", new { orderId = newOrderId, amount = totalAmount, method = selectedMethod });
        }

        public IActionResult CheckoutComplete(int orderId, decimal amount, string method)
        {
            ViewBag.OrderId = orderId;
            ViewBag.Amount = amount;
            ViewBag.PaymentMethod = method;
            return View();
        }

        // 🔥 HÀM API MỚI THÊM: Xử lý cập nhật trạng thái đơn hàng sang "Đã thanh toán" dưới SQL Server
        [HttpPost]
        public IActionResult ConfirmPayment(int orderId)
        {
            var order = _context.Set<Order>().Find(orderId);
            if (order != null)
            {
                order.Status = "Đã thanh toán"; // Ghi nhận trạng thái mới vào CSDL
                _context.SaveChanges(); // Lưu thay đổi
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Không tìm thấy đơn hàng!" });
        }
    }
}