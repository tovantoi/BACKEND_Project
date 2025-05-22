using chuyennganh.Application.Response;
using MediatR;

namespace chuyennganh.Application.App.Blogs.Command
{
    public record CreateBlogRequest : IRequest<ServiceResponse>
    {
        public string? Title { get; set; }
        public string? Slug { get; set; }   
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? CoverImage { get; set; } 
        public string? VideoUrl { get; set; }
    }
}
