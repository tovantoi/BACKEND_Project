using AutoMapper;
using chuyennganh.Application.App.ProductApp.Command;
using chuyennganh.Application.App.ProductApp.Validators;
using chuyennganh.Application.Repositories.CategoryRepo;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ServiceResponse>
{
    private readonly IProductRepository productRepository;
    private readonly ICategoryRepository categoryRepository;
    private readonly IMapper mapper;
    private readonly ILogger<CreateProductHandler> logger;
    private readonly IFileService fileService;

    public CreateProductHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IMapper mapper,
        ILogger<CreateProductHandler> logger,
        IFileService fileService)
    {
        this.productRepository = productRepository;
        this.categoryRepository = categoryRepository;
        this.mapper = mapper;
        this.logger = logger;
        this.fileService = fileService;
    }

    public async Task<ServiceResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await using (var transaction = productRepository.BeginTransaction())
        {
            try
            {
                if (request.CategoryIds is not null && request.CategoryIds.Any()) await categoryRepository.CheckIdsExistAsync(request.CategoryIds.ToList());

                var dubCategoryId = request.CategoryIds!.GroupBy(id => id).Where(g => g.Count() > 1).Select(g => g.Key).ToList();

                var validator = new CreateProductValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                var product = mapper.Map<Product>(request);

                productRepository.Create(product);
                await productRepository.SaveChangeAsync();
                product.ProductCategories = request.CategoryIds?.Distinct().Select(categoryId => new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = categoryId
                }).ToList();
              
                if (request.ImageData is not null)
                {
                    var uploadFile = new UploadFileRequest
                    {
                        Content = request.ImageData,
                        AssetType = AssetType.Product,
                        Suffix = product.Id.ToString(),
                    };
                    product.ImagePath = await fileService.UploadFileAsync(uploadFile);
                }
                await productRepository.SaveChangeAsync(cancellationToken);
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
