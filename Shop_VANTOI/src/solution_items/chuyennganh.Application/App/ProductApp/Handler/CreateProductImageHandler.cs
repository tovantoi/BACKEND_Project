using AutoMapper;
using chuyennganh.Application.App.ProductApp.Command;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class CreateProductImageHandler : IRequestHandler<CreateProductImageCommand, ServiceResponse>
{
    private readonly IProductRepository productRepository;
    private readonly IProductImageRepository productImageRepository;
    private readonly IMapper mapper;
    private readonly ILogger<CreateProductImageHandler> logger;
    private readonly IFileService fileService;

    public CreateProductImageHandler(
        IProductRepository productRepository,
        IProductImageRepository productImageRepository,
        IMapper mapper,
        ILogger<CreateProductImageHandler> logger,
        IFileService fileService)
    {
        this.productRepository = productRepository;
        this.productImageRepository = productImageRepository;
        this.mapper = mapper;
        this.logger = logger;
        this.fileService = fileService;
    }

    public async Task<ServiceResponse> Handle(CreateProductImageCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = productImageRepository.BeginTransaction();
        try
        {
            var product = await productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return ServiceResponse.Failure("Không tìm thấy sản phẩm.");
            }
            var productImage = mapper.Map<ProductImage>(request);
            productImageRepository.Create(productImage);
            await productImageRepository.SaveChangeAsync(cancellationToken);
            if (request.ImageUrl is not null)
            {
                var uploadFile = new UploadFileRequest
                {
                    Content = request.ImageUrl,
                    AssetType = AssetType.Product,
                    Suffix = $"{request.ProductId}_{Guid.NewGuid()}"
                };
                productImage.ImageUrl = await fileService.UploadFileAsync(uploadFile);
            }
            await productImageRepository.SaveChangeAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return ServiceResponse.Success("Thêm ảnh sản phẩm thành công.");
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
