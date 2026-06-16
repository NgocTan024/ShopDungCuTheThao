using Microsoft.AspNetCore.Mvc;
using SportShop.Models.Interfaces;

namespace SportShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        public HomeController(IProductRepository productRepository) => _productRepository = productRepository;

        public IActionResult Index() => View(_productRepository.GetTrendingProducts());
    }
}