using System.Collections.Generic;

namespace SportShop.Models.Interfaces
{
    public interface IOrderRepository
    {
        void PlaceOrder(Order order);

        // ĐĂNG KÝ HÀM NÀY ĐỂ CONTROLLER HẾT BÁO ĐỎ:
        IEnumerable<Order> GetOrdersByEmail(string email);
    }
}