using chuyennganh.Domain.Enumerations;

namespace chuyennganh.Application.App.DTOs
{
    public class ProductReviewDetailDTO
    {
        public int? Id { get; set; }
        public int? ProductId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }

        public OrderStatus? Status { get; set; }
        public CustomerAddressDTO? Address { get; set; }
        public CustomerDTO? CustomerDTO { get; set; }

        public ICollection<OrderItemDTO>? OrderItems { get; set; }
    }
}
