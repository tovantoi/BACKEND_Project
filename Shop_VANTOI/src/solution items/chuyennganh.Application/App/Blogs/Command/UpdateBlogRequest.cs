using chuyennganh.Application.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace chuyennganh.Application.App.Blogs.Command
{
    public record UpdateBlogRequest : IRequest<ServiceResponse>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; } 
        public string? CoverImage { get; set; }
        public string? VideoUrl { get; set; }
        public bool? IsActive { get; set; }
    }
}
