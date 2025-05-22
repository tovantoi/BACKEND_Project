using AutoMapper;
using chuyennganh.Application.App.Blogs.Command;
using chuyennganh.Application.App.Blogs.Validators;
using chuyennganh.Application.Repositories.BlogRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.ExceptionEx;
using chuyennganh.Domain.Shared;
using MediatR;

namespace chuyennganh.Application.App.Blogs.Handler
{
    public class UpdateBlogRequestHandler : IRequestHandler<UpdateBlogRequest, ServiceResponse>
    {
        private readonly IBlogRepository blogRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public UpdateBlogRequestHandler(IBlogRepository blogRepository, IMapper mapper, IFileService fileService)
        {
            this.blogRepository = blogRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<ServiceResponse> Handle(UpdateBlogRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = blogRepository.BeginTransaction())
            {
                try
                {
                    var validator = new UpdateBlogRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var blogs = await blogRepository.GetByIdAsync(request.Id!);
                    if (blogs is null) blogs.ThrowNotFound();
                    blogs!.Content = request.Content ?? blogs.Content;
                    blogs.Description = request.Description ?? blogs.Description;
                    blogs.Title = request.Title ?? blogs.Title;
                    blogs.IsActive = request.IsActive ?? blogs.IsActive;
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
                    await blogRepository.UpdateAsync(blogs);
                    await blogRepository.SaveChangeAsync();
                    await transaction.CommitAsync(cancellationToken);
                    return ServiceResponse.Success("Cập nhật thành công");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}
