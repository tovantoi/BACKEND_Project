using AutoMapper;
using chuyennganh.Application.App.CategoryApp.Command;
using chuyennganh.Application.App.CategoryApp.Validators;
using chuyennganh.Application.Repositories.CategoryRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace chuyennganh.Application.App.CategoryApp.Handler
{
    public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, ServiceResponse>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public CreateCategoryRequestHandler(ICategoryRepository categoryRepository, IMapper mapper, IFileService fileService)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<ServiceResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = categoryRepository.BeginTransaction())
            {
                try
                {
                    var validator = new CreateCategoryRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var category = mapper.Map<Category>(request);
                    categoryRepository.Create(category);
                    await categoryRepository.SaveChangeAsync();
                    if (request.ImageData is not null)
                    {
                        var uploadFile = new UploadFileRequest
                        {
                            Content = request.ImageData,
                            AssetType = AssetType.Category,
                            Suffix = category.Id.ToString(),
                        };
                        category.ImagePath = await fileService.UploadFileAsync(uploadFile);
                    }

                    await categoryRepository.SaveChangeAsync();
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