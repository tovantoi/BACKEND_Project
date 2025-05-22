using AutoMapper;
using chuyennganh.Application.App.CategoryApp.Command;
using chuyennganh.Application.App.CategoryApp.Validators;
using chuyennganh.Application.Repositories.CategoryRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.ExceptionEx;
using chuyennganh.Domain.Shared;
using MediatR;

namespace NhaThuoc.Application.Handlers.Category
{
    public class UpdateCategoryRequestHandler : IRequestHandler<UpdateCategoryRequest, ServiceResponse>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public UpdateCategoryRequestHandler(ICategoryRepository categoryRepository, IMapper mapper, IFileService fileService)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<ServiceResponse> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = categoryRepository.BeginTransaction())
            {
                try
                {
                    var validator = new UpdateCategoryRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var category = await categoryRepository.GetByIdAsync(request.Id!);
                    if (category is null) category.ThrowNotFound();
                    category!.Name = request.Name ?? category.Name;
                    category.Description = request.Description ?? category.Description;
                    category.ParentId = request.ParentId ?? category.ParentId;
                    category.IsActive = request.IsActive ?? category.IsActive;
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
                    await categoryRepository.UpdateAsync(category);
                    await categoryRepository.SaveChangeAsync();
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