using chuyennganh.Domain.Base;

namespace chuyennganh.Domain.Entities
{
    public class ProductSize : BaseEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? SizeLabel { get; set; }
        public int StockQuantity { get; set; }

        public Product? Product { get; set; }
    }

}
