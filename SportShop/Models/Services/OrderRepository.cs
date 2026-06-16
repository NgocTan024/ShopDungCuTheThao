using SportShop.Data;
using SportShop.Models.Interfaces;

namespace SportShop.Models.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SportShopDbContext _context;
        private readonly IShoppingCartRepository _cart;
        public OrderRepository(SportShopDbContext context, IShoppingCartRepository cart) { _context = context; _cart = cart; }

        public void PlaceOrder(Order order)
        {
            var cartItems = _cart.GetAllShoppingCartItems();
            order.OrderDetails = new List<OrderDetail>();
            foreach (var item in cartItems)
            {
                order.OrderDetails.Add(new OrderDetail { Quantity = item.Qty, ProductId = item.Product!.Id, Price = item.Product.Price });
            }
            order.OrderPlaced = DateTime.Now;
            order.OrderTotal = _cart.GetShoppingCartTotal();
            _context.Order.Add(order);
            _context.SaveChanges();
        }
    }
}