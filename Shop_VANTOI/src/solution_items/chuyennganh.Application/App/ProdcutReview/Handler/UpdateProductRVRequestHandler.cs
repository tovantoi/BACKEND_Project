using AutoMapper;
using chuyennganh.Application.App.ProdcutReview.Command;
using chuyennganh.Application.App.ProdcutReview.Validators;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.ExceptionEx;
using chuyennganh.Domain.Shared;
using MediatR;

namespace chuyennganh.Application.App.ProdcutReview.Handler
{
    public class UpdateProductRVRequestHandler : IRequestHandler<UpdateReviewRequest, ServiceResponse>
    {
        private readonly IProductReviewRepository productReviewRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public UpdateProductRVRequestHandler(IProductReviewRepository productReviewRepository, IMapper mapper, IFileService fileService)
        {
            this.productReviewRepository = productReviewRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }
        public async Task<ServiceResponse> Handle(UpdateReviewRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = productReviewRepository.BeginTransaction())
            {
                try
                {
                    var validator = new UpdateProductReviewRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var productReview = await productReviewRepository.GetByIdAsync(request.Id!);
                    if (productReview is null) productReview.ThrowNotFound();
                    productReview.ProductId = request.ProductId ?? productReview.ProductId;
                    productReview.UserId = request.UserId ?? productReview.UserId;
                    productReview.Comment = request.Comment ?? productReview.Comment;
                    productReview.Rating = request.Rating ?? productReview.Rating;
                    productReview.ImageUrl = request.ImageUrl ?? productReview.ImageUrl;
                    productReview.VideoUrl = request.VideoUrl ?? productReview.VideoUrl;
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
                    await productReviewRepository.UpdateAsync(productReview);
                    await productReviewRepository.SaveChangeAsync();
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
