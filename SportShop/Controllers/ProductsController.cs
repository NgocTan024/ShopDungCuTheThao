using Microsoft.AspNetCore.Mvc;
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
            return View(product);
        }
    }
}