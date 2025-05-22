using chuyennganh.Application.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace chuyennganh.Application.App.ProductApp.Command
{
    public record CreateProductCommand : IRequest<ServiceResponse>
    {
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
        public string? ImageData { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoAlias { get; set; }
        public bool? IsActive { get; set; } = false;

        [JsonIgnore]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public ICollection<int>? CategoryIds { get; set; } = new List<int>();
    }
}
