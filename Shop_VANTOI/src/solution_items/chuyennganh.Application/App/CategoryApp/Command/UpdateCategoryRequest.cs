using chuyennganh.Application.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace chuyennganh.Application.App.CategoryApp.Command
{
    public record UpdateCategoryRequest : IRequest<ServiceResponse>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageData { get; set; }
        public bool? IsActive { get; set; }
    }
}
