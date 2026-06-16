using Microsoft.AspNetCore.Mvc;
using SportShop.Models;
using SportShop.Models.Interfaces;

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

        public IActionResult Checkout() => View();

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            var items = _cartRepository.GetAllShoppingCartItems();
            if (items.Count == 0) { ModelState.AddModelError("", "Giỏ hàng của bạn đang trống!"); return View(order); }

            _orderRepository.PlaceOrder(order);
            _cartRepository.ClearCart();
            HttpContext.Session.SetInt32("CartCount", 0);
            return RedirectToAction("CheckoutComplete");
        }

        public IActionResult CheckoutComplete() => View();
    }
}