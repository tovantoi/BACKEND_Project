using chuyennganh.Domain.Base;
using System.Text.Json.Serialization;

namespace chuyennganh.Domain.Entities
{
    public class Product : BaseEntity
    {
        public int? Id { get; set; }

        public string? ProductName { get; set; }
        public double? RegularPrice { get; set; }
        public double? DiscountPrice { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? Material { get; set; }
        public string? Gender { get; set; }
        public string? Packaging { get; set; }
        public string? Origin { get; set; }
        public string? Manufacturer { get; set; }
        public string? ImagePath { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoAlias { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }

        [JsonIgnore]
        public ICollection<ProductCategory>? ProductCategories { get; set; }
    }
}