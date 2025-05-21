using AutoMapper;
using chuyennganh.Application.App.ProdcutReview.Command;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.ProdcutReview.Handler
{
    public class GetAllProductRVRequestHandler : IRequestHandler<GetAllReviewRequest, List<ProductReview>>
    {
        private readonly IProductReviewRepository productReviewRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public GetAllProductRVRequestHandler(IProductReviewRepository productReviewRepository, IMapper mapper, IFileService fileService)
        {
            this.productReviewRepository = productReviewRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }
        public async Task<List<ProductReview>> Handle(GetAllReviewRequest request, CancellationToken cancellationToken)
        {
            var productReviews = productReviewRepository.FindAll().ToList();

            var employees = await Task.Run(() =>
            {
                return productReviews
                    .Select(c => new
                    {
                        ProductReview = c,
                        ImageUrl = string.IsNullOrEmpty(c.ImageUrl) ? null : fileService.GetFullPathFileServer(c.ImageUrl),
                        VideoUrl = string.IsNullOrEmpty(c.VideoUrl) ? null : fileService.GetFullPathFileServer(c.VideoUrl)
                    })
                    .ToList();
            }, cancellationToken);

            var result = employees
                .Select(x =>
                {
                    x.ProductReview.ImageUrl = x.ImageUrl;
                    x.ProductReview.VideoUrl = x.VideoUrl;
                    return x.ProductReview;
                })
                .ToList();

            // Trả về result đã cập nhật thay vì productReviews gốc
            return result;
        }

    }
}
