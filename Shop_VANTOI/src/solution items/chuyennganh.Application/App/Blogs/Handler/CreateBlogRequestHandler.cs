using AutoMapper;
using chuyennganh.Application.App.Blogs.Command;
using chuyennganh.Application.App.Blogs.Validators;
using chuyennganh.Application.Repositories.BlogRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace chuyennganh.Application.App.Blogs.Handler
{
    public class CreateBlogRequestHandler : IRequestHandler<CreateBlogRequest, ServiceResponse>
    {
        private readonly IBlogRepository blogRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public CreateBlogRequestHandler(IBlogRepository blogRepository, IMapper mapper, IFileService fileService)
        {
            this.blogRepository = blogRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<ServiceResponse> Handle(CreateBlogRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = blogRepository.BeginTransaction())
            {
                try
                {
                    var validator = new CreateBlogRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var blogs = mapper.Map<Blog>(request);
                    blogRepository.Create(blogs);
                    await blogRepository.SaveChangeAsync();
                    if (request.CoverImage is not null)
                    {
                        var uploadFile = new UploadFileRequest
                        {
                            Content = request.CoverImage,
                            AssetType = AssetType.Blog,
                            Suffix = blogs.Id.ToString(),
                        };
                        blogs.CoverImage = await fileService.UploadFileAsync(uploadFile);
                    }
                    if (request.VideoUrl is not null)
                    {
                        var uploadFile = new UploadFileRequest
                        {
                            Content = request.VideoUrl,
                            AssetType = AssetType.Blog,
                            Suffix = blogs.Id.ToString(),
                        };
                        blogs.VideoUrl = await fileService.UploadFileAsync(uploadFile);
                    }
                    await blogRepository.SaveChangeAsync();
                    await transaction.CommitAsync(cancellationToken);
                    return ServiceResponse.Success("Tạo thành công");
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new ServiceResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                    };
                }
            }
        }
    }
}
