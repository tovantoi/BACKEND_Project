using chuyennganh.Application.Response;
using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.Blogs.Command
{
    public record GetDetailBlogRequest : IRequest<Blog>
    {
        public int? Id { get; set; }
    }
}
