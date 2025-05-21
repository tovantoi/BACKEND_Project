using AutoMapper;
using chuyennganh.Application.App.ProdcutReview.Command;
using chuyennganh.Application.App.ProdcutReview.Validators;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace chuyennganh.Application.App.ProdcutReview.Handler
{
    public class CreateProductRVRequestHandler : IRequestHandler<CreateReviewRequest, ServiceResponse>
    {
        private readonly IProductReviewRepository productReviewRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        private readonly IOrderRepository orderRepository;
        public CreateProductRVRequestHandler(IProductReviewRepository productReviewRepository, IMapper mapper, IFileService fileService, IOrderRepository orderRepository)
        {
            this.productReviewRepository = productReviewRepository;
            this.mapper = mapper;
            this.fileService = fileService;
            this.orderRepository = orderRepository;
        }

        public async Task<ServiceResponse> Handle(CreateReviewRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = productReviewRepository.BeginTransaction())
            {
                try
                {
                    var validator = new CreateProductReviewRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var productReview = mapper.Map<ProductReview>(request);
                    productReviewRepository.Create(productReview);
                    // ⚠️ Kiểm tra xem người dùng đã mua sản phẩm chưa
                    var hasPurchased = await orderRepository.HasUserPurchasedProductAsync(request.UserId, request.ProductId);
                    if (!hasPurchased)
                    {
                        return new ServiceResponse
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = "Bạn cần mua sản phẩm trước khi đánh giá."
                        };
                    }

                    await productReviewRepository.SaveChangeAsync();
                    if (request.ImageUrl is not null)
                    {
                        var uploadFile = new UploadFileRequest
                        {
                            Content = request.ImageUrl,
                            AssetType = AssetType.Review,
                            Suffix = productReview.Id.ToString(),
                        };
                        productReview.ImageUrl = await fileService.UploadFileAsync(uploadFile);
                    }
                    if (request.VideoUrl is not null)
                    {
                        var uploadFile = new UploadFileRequest
                        {
                            Content = request.VideoUrl,
                            AssetType = AssetType.Review,
                            Suffix = productReview.Id.ToString(),
                        };
                        productReview.VideoUrl = await fileService.UploadFileAsync(uploadFile);
                    }
                    await productReviewRepository.SaveChangeAsync();
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