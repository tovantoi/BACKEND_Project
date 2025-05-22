using chuyennganh.Domain.Base;
using System.Text.Json.Serialization;

namespace chuyennganh.Domain.Entities
{
    public class Category : BaseEntity
    {
        public int? Id { get; set; }

        public int? ParentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }

        [JsonIgnore]
        public ICollection<ProductCategory>? ProductCategories { get; set; }

    }
}
