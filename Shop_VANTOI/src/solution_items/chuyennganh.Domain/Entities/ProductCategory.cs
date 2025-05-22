using chuyennganh.Domain.Base;

namespace chuyennganh.Domain.Entities
{
    public class ProductCategory : BaseEntity
    {
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; } = default!;
    }
}