using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters; // THÊM DÒNG NÀY ĐỂ DÙNG BỘ LỌC KIỂM TRA
using SportShop.Data;
using SportShop.Models;
using SportShop.Models.Interfaces;
using System.Linq;

namespace SportShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly SportShopDbContext _context;

        public AdminController(IProductRepository productRepository, SportShopDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        // 🔥 CHỐT CHẶN BẢO MẬT TOÀN CỤC: Chạy trước TẤT CẢ các hàm bên dưới
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            // Nếu người dùng chưa đăng nhập HOẶC email đăng nhập không phải admin@gmail.com
            if (user.Identity == null || !user.Identity.IsAuthenticated || user.Identity.Name != "admin@gmail.com")
            {
                // Lập tức chặn đứng và điều hướng thẳng về trang Login của AccountController
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }

        // 1. Danh sách sản phẩm quản trị (Đã được bảo vệ)
        public IActionResult Index()
        {
            var products = _productRepository.GetAllProducts().OrderBy(p => p.Id);
            return View(products);
        }

        // 2. Mở form thêm mới (Đã được bảo vệ)
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Set<Product>().Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // 3. Mở form sửa giá / thông tin (Đã được bảo vệ)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetProductDetail(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Set<Product>().Update(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // 4. Xóa sản phẩm (Đã được bảo vệ)
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _context.Set<Product>().Find(id);
            if (product != null)
            {
                _context.Set<Product>().Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");

        }
        public IActionResult Sales()
        {
            // Lấy toàn bộ danh sách đơn hàng, sắp xếp theo thời gian mới nhất
            var allOrders = _context.Set<Order>().OrderByDescending(o => o.OrderPlaced).ToList();
            return View(allOrders);
        }
    }
}