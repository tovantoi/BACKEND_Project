using AutoMapper;
using chuyennganh.Application.App.ProdcutReview.Command;
using chuyennganh.Application.Repositories.OrderItemRepo;
using chuyennganh.Application.Repositories.ProductRepo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace chuyennganh.Application.App.ProdcutReview.Handler
{
    public class GetProductReviewSummaryRequestHandler : IRequestHandler<GetProductReviewSummaryRequest, ProductReviewSummaryResponse>
    {
        private readonly IProductReviewRepository productReviewRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly IMapper mapper;

        public GetProductReviewSummaryRequestHandler(IProductReviewRepository productReviewRepository, IMapper mapper, IOrderItemRepository orderItemRepository)
        {
            this.productReviewRepository = productReviewRepository;
            this.mapper = mapper;
            this.orderItemRepository = orderItemRepository;
        }

        public async Task<ProductReviewSummaryResponse> Handle(GetProductReviewSummaryRequest request, CancellationToken cancellationToken)
        {
            // Query review theo ProductId
            var reviews = productReviewRepository.FindAll()
                .Where(r => r.ProductId == request.ProductId);

            var totalReviews = await reviews.CountAsync(cancellationToken);
            var averageRating = totalReviews > 0
                ? await reviews.AverageAsync(r => r.Rating, cancellationToken)
                : 0;
            var totalSold = await orderItemRepository.FindAll()
                .Where(oi => oi.ProductId == request.ProductId)
                .SumAsync(oi => oi.Quantity ?? 0, cancellationToken);
            return new ProductReviewSummaryResponse
            {
                TotalReviews = totalReviews,
                AverageRating = Math.Round(averageRating, 1), 
                TotalSold = totalSold
            };
        }
    }
}
