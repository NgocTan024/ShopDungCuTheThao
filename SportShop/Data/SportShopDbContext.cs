using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportShop.Models;

namespace SportShop.Data
{
    public class SportShopDbContext : IdentityDbContext
    {
        public SportShopDbContext(DbContextOptions<SportShopDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);
            modelBuilder.Entity<OrderDetail>().Property(o => o.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Order>().Property(o => o.OrderTotal).HasPrecision(18, 2);

            // NẠP SẴN HỆ THỐNG 20 SẢN PHẨM SIÊU THỊ THỂ THAO
            modelBuilder.Entity<Product>().HasData(
    // === DANH MỤC: GYM (Id: 1 -> 4) ===
    new Product { Id = 1, Name = "Tạ Tay Cao Su 5kg", Price = 150000, Detail = "Tạ đúc nguyên khối bọc cao su cao cấp chống trầy xước sàn", Category = "Gym", ImageUrl = "/assets/images/ta-tay.jpg", IsTrendingProduct = true },
    new Product { Id = 2, Name = "Ghế Tập Bụng Đa Năng", Price = 1250000, Detail = "Ghế dốc tập cơ bụng, cơ ngực có thể gấp gọn tiện lợi", Category = "Gym", ImageUrl = "/assets/images/ghe-tap.jpg", IsTrendingProduct = true },
    new Product { Id = 3, Name = "Con Lăn Bụng 3 Bánh", Price = 120000, Detail = "Hỗ trợ tập cơ bụng sáu múi hiệu quả tại nhà kèm thảm lót gối", Category = "Gym", ImageUrl = "/assets/images/con-lan.jpg", IsTrendingProduct = false },
    new Product { Id = 4, Name = "Dây Ngũ Sắc Kháng Lực", Price = 180000, Detail = "Bộ 5 dây lực kéo khác nhau hỗ trợ tập tạ và căng cơ toàn thân", Category = "Gym", ImageUrl = "/assets/images/day-ngu-sac.jpg", IsTrendingProduct = false },

    // === DANH MỤC: YOGA & PILATES (Id: 5 -> 8) ===
    new Product { Id = 5, Name = "Thảm Yoga Định Tuyến", Price = 350000, Detail = "Thảm chất liệu TPE 6mm thân thiện môi trường, chống trượt tuyệt đối", Category = "Yoga", ImageUrl = "/assets/images/tham-yoga.jpg", IsTrendingProduct = true },
    new Product { Id = 6, Name = "Vòng Tập Yoga Chịu Lực", Price = 280000, Detail = "Khung nhựa ABS chịu lực 150kg mở vai uốn lưng chuyên sâu", Category = "Yoga", ImageUrl = "/assets/images/vong-yoga.jpg", IsTrendingProduct = false },
    new Product { Id = 7, Name = "Bóng Yoga PVC Trơn 65cm", Price = 160000, Detail = "Bóng dầy dặn chống nổ giữ form tốt tặng kèm bơm tay siêu tốc", Category = "Yoga", ImageUrl = "/assets/images/bong-yoga.jpg", IsTrendingProduct = false },
    new Product { Id = 8, Name = "Gạch Xốp Hỗ Trợ Yoga", Price = 60000, Detail = "Khối xốp EVA mật độ cao hỗ trợ giữ thăng bằng các tư thế khó", Category = "Yoga", ImageUrl = "/assets/images/gach-yoga.jpg", IsTrendingProduct = false },

    // === DANH MỤC: CHẠY BỘ (Id: 9 -> 12) ===
    new Product { Id = 9, Name = "Giày Chạy Siêu Nhẹ Air", Price = 850000, Detail = "Đế cao su giảm chấn đàn hồi tốt, vải mesh thoáng khí cho runner", Category = "Chạy bộ", ImageUrl = "/assets/images/giay-chay.jpg", IsTrendingProduct = true },
    new Product { Id = 10, Name = "Đai Đeo Điện Thoại Runner", Price = 75000, Detail = "Chất liệu co giãn chống nước tốt có ngăn chứa chìa khóa xe", Category = "Chạy bộ", ImageUrl = "/assets/images/dai-chay.jpg", IsTrendingProduct = false },
    new Product { Id = 11, Name = "Bình Nước Thể Thao 1L", Price = 95000, Detail = "Nhựa Tritan an toàn không BPA, có vạch chia dung tích tiện lợi", Category = "Chạy bộ", ImageUrl = "/assets/images/binh-nuoc.jpg", IsTrendingProduct = false },
    new Product { Id = 12, Name = "Tất Thể Thao Cổ Ngắn (3 Đôi)", Price = 50000, Detail = "Chất cotton dày dặn thấm hút mồ hôi chống phồng rộp gót chân", Category = "Chạy bộ", ImageUrl = "/assets/images/tat-chay.jpg", IsTrendingProduct = false },

    // === DANH MỤC: BÓNG ĐÁ (Id: 13 -> 16) ===
    new Product { Id = 13, Name = "Bóng Đá Thi Đấu Số 5", Price = 450000, Detail = "Đạt tiêu chuẩn thi đấu quốc tế, chất da PU bền bỉ đường may kín", Category = "Bóng đá", ImageUrl = "/assets/images/bong-da.jpg", IsTrendingProduct = true },
    new Product { Id = 14, Name = "Găng Tay Thủ Môn Có Xương", Price = 320000, Detail = "Lòng găng latex bám dính tốt có xương bảo vệ chống lật ngón tay", Category = "Bóng đá", ImageUrl = "/assets/images/gang-tay.jpg", IsTrendingProduct = false },
    new Product { Id = 15, Name = "Băng Bảo Vệ Ống Quyển", Price = 45000, Detail = "Nhựa PP chịu lực lót mút EVA êm ái bảo vệ chân giảm va chạm", Category = "Bóng đá", ImageUrl = "/assets/images/boc-ong-dong.jpg", IsTrendingProduct = false },
    new Product { Id = 16, Name = "Băng Gối Thể Thao Bản Rộng", Price = 110000, Detail = "Hỗ trợ cố định khớp gối, dây đai tăng cường giảm áp lực dây chằng", Category = "Bóng đá", ImageUrl = "/assets/images/bang-goi.jpg", IsTrendingProduct = false },

    // === DANH MỤC: CẦU LÔNG (Id: 17 -> 20) ===
    new Product { Id = 17, Name = "Vợt Cầu Lông Khung Carbon", Price = 680000, Detail = "Khung carbon nguyên khối siêu nhẹ lực căng tốt trợ lực cho cổ tay", Category = "Cầu lông", ImageUrl = "/assets/images/vot-cau.jpg", IsTrendingProduct = true },
    new Product { Id = 18, Name = "Hộp Cầu Lông 12 Quả", Price = 190000, Detail = "Cầu Thành Công độ bền lông cầu cao đường bay đầm quỹ đạo chuẩn ổn định", Category = "Cầu lông", ImageUrl = "/assets/images/hop-cau.jpg", IsTrendingProduct = false },
    new Product { Id = 19, Name = "Bao Vợt Cầu Lông 2 Ngăn", Price = 150000, Detail = "Vải chống thấm nước dày dặn đựng vừa 3 cây vợt và giày quần áo", Category = "Cầu lông", ImageUrl = "/assets/images/bao-vot.jpg", IsTrendingProduct = false },
    new Product { Id = 20, Name = "Cuộn Quấn Cán Vợt (Combo 5 chiếc)", Price = 35000, Detail = "Chất liệu cao su non siêu bám tay thấm hút mồ hôi chống trơn trượt", Category = "Cầu lông", ImageUrl = "/assets/images/quan-can.jpg", IsTrendingProduct = false }
);
        }
    }
}