using chuyennganh.Application.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace chuyennganh.Application.App.CategoryApp.Command
{
    public record CreateCategoryRequest : IRequest<ServiceResponse>
    {
        public int? ParentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageData { get; set; }
        public bool? IsActive { get; set; } = false;
        [JsonIgnore]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}