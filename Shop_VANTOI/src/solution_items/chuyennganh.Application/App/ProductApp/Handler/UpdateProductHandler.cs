using AutoMapper;
using chuyennganh.Application.App.ProductApp.Command;
using chuyennganh.Application.App.ProductApp.Validators;
using chuyennganh.Application.Repositories.CategoryRepo;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.ExceptionEx;
using chuyennganh.Domain.Shared;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Handler
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ServiceResponse>
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductCategoryRepository productCategoryRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public UpdateProductHandler(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository, IProductCategoryRepository productCategoryRepository, IFileService fileService)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
            this.productCategoryRepository = productCategoryRepository;
            this.fileService = fileService;
        }
        public async Task<ServiceResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            await using (var transaction = categoryRepository.BeginTransaction())
            {
                try
                {
                    var validator = new UpdateProductValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var product = await productRepository.GetByIdAsync(request.Id);
                    if (product is null) product.ThrowNotFound();
                    if (request.CategoryIds is not null && request.CategoryIds.Any()) await categoryRepository.CheckIdsExistAsync(request.CategoryIds.ToList());

                    var dubCategoryId = request.CategoryIds.GroupBy(id => id).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                    if (dubCategoryId.Any()) dubCategoryId.ThrowConflict();

                    product.ProductName = request.ProductName ?? product.ProductName;
                    product.RegularPrice = request.RegularPrice ?? product.RegularPrice;
                    product.DiscountPrice = request.DiscountPrice ?? product.DiscountPrice;
                    product.Description = request.Description ?? product.Description;
                    product.Brand = request.Brand ?? product.Brand;
                    product.Size = request.Size ?? product.Size;
                    product.Color = request.Color ?? product.Color;
                    product.Material = request.Material ?? product.Material;
                    product.Gender = request.Gender ?? product.Gender;
                    product.Packaging = request.Packaging ?? product.Packaging;
                    product.Origin = request.Origin ?? product.Origin;
                    product.Manufacturer = request.Manufacturer ?? product.Manufacturer;
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
                    product.SeoAlias = request.SeoAlias ?? product.SeoAlias;
                    product.SeoTitle = request.SeoTitle ?? product.SeoTitle;
                    product.IsActive = request.IsActive ?? product.IsActive;


                    if (request.CategoryIds is not null && request.CategoryIds.Any())
                    {
                        // Lấy danh sách danh mục hiện tại
                        var existingProductCategories = productCategoryRepository.FindAll(x => x.ProductId == request.Id).ToList();

                        // Xóa danh mục không còn tồn tại
                        productCategoryRepository.RemoveMultiple(existingProductCategories);

                        // Thêm mới danh mục
                        foreach (var categoryId in request.CategoryIds.Distinct())
                        {
                            await productCategoryRepository.AddAsync(new ProductCategory
                            {
                                ProductId = product.Id,
                                CategoryId = categoryId
                            });
                        }
                    }

                    await productRepository.UpdateAsync(product);
                    await productRepository.SaveChangeAsync();
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
