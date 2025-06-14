using chuyennganh.Domain.Base;
using System.Text.Json.Serialization;

namespace chuyennganh.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public int Id { get; set; }

        public int ProductId { get; set; } 

        public string? ImageUrl { get; set; }

        public string? Color { get; set; }
        public int? StockQuantity { get; set; }
        public int SortOrder { get; set; } = 0;

        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
