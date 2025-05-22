using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.Blogs.Command
{
    public class GetAllBlogRequest : IRequest<List<Blog>>
    {
    }
}
