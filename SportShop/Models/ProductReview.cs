using System;

namespace SportShop.Models
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string? UserEmail { get; set; }
        public int Rating { get; set; } // Lưu từ 1 đến 5 sao
        public string? Comment { get; set; }
        public DateTime DateReviewed { get; set; }
    }
}