using AutoMapper;
using chuyennganh.Application.App.ProdcutReview.Command;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.ProdcutReview.Handler
{
    public class GetDetailProductRVRequestHandler : IRequestHandler<GetDetailReviewRequest, List<ProductReview>>
    {
        private readonly IProductReviewRepository productReviewRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public GetDetailProductRVRequestHandler(IProductReviewRepository productReviewRepository, IMapper mapper, IFileService fileService)
        {
            this.productReviewRepository = productReviewRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<List<ProductReview>> Handle(GetDetailReviewRequest request, CancellationToken cancellationToken)
        {          
            var reviews = productReviewRepository.FindAll(x => x.ProductId == request.ProductId).ToList();
            if (reviews is null) reviews.ThrowNotFound();
            // ✅ Xử lý từng review
            foreach (var review in reviews)
            {
                review.ImageUrl = string.IsNullOrEmpty(review.ImageUrl)
                    ? null
                    : fileService.GetFullPathFileServer(review.ImageUrl);

                review.VideoUrl = string.IsNullOrEmpty(review.VideoUrl)
                    ? null
                    : fileService.GetFullPathFileServer(review.VideoUrl);
            }
            
            return reviews;
        }
    }
}
