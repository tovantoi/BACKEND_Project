using AutoMapper;
using chuyennganh.Application.App.Blogs.Command;
using chuyennganh.Application.Repositories.BlogRepo;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.Blogs.Handler
{
    public class GetDetailBlogRequestHandler : IRequestHandler<GetDetailBlogRequest, Blog>
    {
        private readonly IBlogRepository blogRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public GetDetailBlogRequestHandler(IBlogRepository blogRepository, IMapper mapper, IFileService fileService)
        {
            this.blogRepository = blogRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<Blog> Handle(GetDetailBlogRequest request, CancellationToken cancellationToken)
        {
            Blog? blog = await blogRepository.GetByIdAsync(request.Id);
            if (blog is null) blog.ThrowNotFound();
            blog.CoverImage = string.IsNullOrEmpty(blog.CoverImage)
            ? null
                : fileService.GetFullPathFileServer(blog.CoverImage);
            blog.VideoUrl = string.IsNullOrEmpty(blog.VideoUrl)
            ? null
                : fileService.GetFullPathFileServer(blog.VideoUrl);
            return mapper.Map<Blog>(blog);
        }
    }
}
