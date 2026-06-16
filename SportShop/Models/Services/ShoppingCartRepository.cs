using SportShop.Data;
using SportShop.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SportShop.Models.Services
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly SportShopDbContext _context;
        public ShoppingCartRepository(SportShopDbContext context) => _context = context;

        public List<ShoppingCartItem>? ShoppingCartItems { get; set; }
        public string? ShoppingCartId { get; set; }

        public static ShoppingCartRepository GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            var context = services.GetService<SportShopDbContext>() ?? throw new Exception("Error DB");
            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
            session?.SetString("CartId", cartId);
            return new ShoppingCartRepository(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(Product product)
        {
            var item = _context.ShoppingCartItems.FirstOrDefault(s => s.Product!.Id == product.Id && s.ShoppingCartId == ShoppingCartId);
            if (item == null)
            {
                _context.ShoppingCartItems.Add(new ShoppingCartItem { ShoppingCartId = ShoppingCartId, Product = product, Qty = 1 });
            }
            else { item.Qty++; }
            _context.SaveChanges();
        }

        public int RemoveFromCart(Product product)
        {
            var item = _context.ShoppingCartItems.FirstOrDefault(s => s.Product!.Id == product.Id && s.ShoppingCartId == ShoppingCartId);
            int localQty = 0;
            if (item != null)
            {
                if (item.Qty > 1) { item.Qty--; localQty = item.Qty; }
                else { _context.ShoppingCartItems.Remove(item); }
            }
            _context.SaveChanges();
            return localQty;
        }

        public List<ShoppingCartItem> GetAllShoppingCartItems() =>
            ShoppingCartItems ??= _context.ShoppingCartItems.Where(s => s.ShoppingCartId == ShoppingCartId).Include(p => p.Product).ToList();

        public void ClearCart()
        {
            var items = _context.ShoppingCartItems.Where(s => s.ShoppingCartId == ShoppingCartId);
            _context.ShoppingCartItems.RemoveRange(items);
            _context.SaveChanges();
        }

        public decimal GetShoppingCartTotal() =>
            _context.ShoppingCartItems.Where(s => s.ShoppingCartId == ShoppingCartId).Select(s => s.Product!.Price * s.Qty).Sum();
    }
}