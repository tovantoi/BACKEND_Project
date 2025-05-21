using chuyennganh.Domain.Entities;

namespace chuyennganh.Application.App.DTOs
{
    public class OrderItemDTO
    {
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public string? ProductName { get; set; }
        public double? DiscountPrice { get; set; }
        public string? ImagePath { get; set; }
        public Product? Product { get; set; }
    }
}