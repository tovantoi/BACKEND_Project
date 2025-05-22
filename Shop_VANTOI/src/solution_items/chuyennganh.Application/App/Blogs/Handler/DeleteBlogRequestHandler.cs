using AutoMapper;
using chuyennganh.Application.App.Blogs.Command;
using chuyennganh.Application.App.Blogs.Validators;
using chuyennganh.Application.Repositories.BlogRepo;
using chuyennganh.Application.Response;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Application.App.Blogs.Handler
{
    public class DeleteBlogRequestHandler : IRequestHandler<DeleteBlogRequest, ServiceResponse>
    {
        private readonly IBlogRepository blogRepository;
        private readonly IMapper mapper;
        private readonly ILogger<DeleteBlogRequestHandler> logger;

        public DeleteBlogRequestHandler(IBlogRepository blogRepository, IMapper mapper, ILogger<DeleteBlogRequestHandler> logger)
        {
            this.blogRepository = blogRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ServiceResponse> Handle(DeleteBlogRequest request, CancellationToken cancellationToken)
        {
            var response = new ServiceResponse();
            var validator = new DeleteBlogRequestValidator();
            validator.ValidateAndThrow(request);
            await using (var transaction = blogRepository.BeginTransaction())
            {
                try
                {
                    var blogs = await blogRepository.GetByIdAsync(request.Id!.Value);
                    if (blogs == null)
                    {
                        response.IsSuccess = false;
                        response.Message = "Blog ID not found";
                        return response;
                    }
                    blogs.IsActive = request.IsActive ?? blogs.IsActive;
                    await blogRepository.UpdateAsync(blogs);
                    await blogRepository.SaveChangeAsync();

                    transaction.Commit();

                    response.IsSuccess = true;
                    response.Message = "Changes IsActive Successful";

                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    response.IsSuccess = false;
                    response.Message = $"An error occurred: {ex.Message}";
                    return response;
                }

            }
        }
    }
}
