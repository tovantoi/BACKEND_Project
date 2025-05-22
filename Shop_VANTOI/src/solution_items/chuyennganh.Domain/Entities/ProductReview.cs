using chuyennganh.Domain.Base;

namespace chuyennganh.Domain.Entities
{
    public class ProductReview : BaseEntity
    {
        public int? Id { get; set; }

        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; } // Số sao (1-5)
        public string? Comment { get; set; }
        public string? ImageUrl { get; set; } // Lưu URL ảnh (nếu có)
        public string? VideoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
