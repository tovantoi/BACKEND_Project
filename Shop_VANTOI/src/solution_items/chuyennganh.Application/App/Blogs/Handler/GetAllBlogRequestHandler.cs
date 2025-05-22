using AutoMapper;
using chuyennganh.Application.App.Blogs.Command;
using chuyennganh.Application.Repositories.BlogRepo;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.Blogs.Handler
{
    public class GetAllBlogRequestHandler : IRequestHandler<GetAllBlogRequest, List<Blog>>
    {
        private readonly IBlogRepository blogRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public GetAllBlogRequestHandler(IBlogRepository blogRepository, IMapper mapper, IFileService fileService)
        {
            this.blogRepository = blogRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<List<Blog>> Handle(GetAllBlogRequest request, CancellationToken cancellationToken)
        {
            var blogs = blogRepository.FindAll().ToList();

            var employees = await Task.Run(() =>
            {
                return blogs
                    .Select(c => new
                    {
                        Blog = c,
                        CoverImage = string.IsNullOrEmpty(c.CoverImage) ? null : fileService.GetFullPathFileServer(c.CoverImage),
                        VideoUrl = string.IsNullOrEmpty(c.VideoUrl) ? null : fileService.GetFullPathFileServer(c.VideoUrl)
                    })
                    .ToList();
            }, cancellationToken);

            var result = employees
                .Select(x =>
                {
                    x.Blog.CoverImage = x.CoverImage;
                    x.Blog.VideoUrl = x.VideoUrl;
                    return x.Blog;
                })
                .ToList();
            return result;
        }

    }
}
