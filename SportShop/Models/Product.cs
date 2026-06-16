namespace SportShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Detail { get; set; } // Mô tả sản phẩm
        public string? Category { get; set; } // Loại: Gym, Yoga, Chạy bộ, Bóng đá...
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsTrendingProduct { get; set; } // Sản phẩm nổi bật ở trang chủ
    }
}