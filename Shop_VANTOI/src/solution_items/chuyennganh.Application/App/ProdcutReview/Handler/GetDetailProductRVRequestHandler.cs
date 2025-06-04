using AutoMapper;
using chuyennganh.Application.App.DTOs;
using chuyennganh.Application.App.ProdcutReview.Command;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace chuyennganh.Application.App.ProdcutReview.Handler
{
    public class GetDetailProductRVRequestHandler : IRequestHandler<GetDetailReviewRequest, List<ProductReviewDetailDTO>>
    {
        private readonly IProductReviewRepository productReviewRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public GetDetailProductRVRequestHandler(
            IProductReviewRepository productReviewRepository,
            IOrderRepository orderRepository,
            IMapper mapper,
            IFileService fileService)
        {
            this.productReviewRepository = productReviewRepository;
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<List<ProductReviewDetailDTO>> Handle(GetDetailReviewRequest request, CancellationToken cancellationToken)
        {
            var reviews = productReviewRepository
                .FindAll(x => x.ProductId == request.ProductId).Include(x => x.Customer)
                .ToList();

            if (reviews == null || !reviews.Any())
                throw new Exception("Không có đánh giá nào.");

            var orderQuery = orderRepository
                .FindAll(o => o.Status.HasValue && o.Status.Value == OrderStatus.Successed
                        && o.OrderItems.Any(oi => oi.ProductId == request.ProductId))
                .Include(o => o.Customer)
                .Include(o => o.CustomerAddress)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product);

            var orders = orderQuery.ToList();

            var result = new List<ProductReviewDetailDTO>();

            foreach (var review in reviews)
            {
                // Tìm order có sản phẩm trùng với review
                var matchingOrder = orders.FirstOrDefault(o =>
                    o.OrderItems.Any(oi => oi.ProductId == review.ProductId)
                );

                var orderItem = matchingOrder?.OrderItems.FirstOrDefault(oi => oi.ProductId == review.ProductId);

                result.Add(new ProductReviewDetailDTO
                {
                    Id = review.Id,
                    ProductId = review.ProductId,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    ImageUrl = fileService.GetFullPathFileServer(review.ImageUrl),
                    VideoUrl = fileService.GetFullPathFileServer(review.VideoUrl),
                    CustomerDTO = new CustomerDTO
                    {
                        FullName = $"{review.Customer.LastName} {review.Customer.FirstName}".Trim(),
                        Email = review.Customer.Email,
                        AvatarImagePath = string.IsNullOrEmpty(review.Customer.AvatarImagePath)
                                ? null
                                : fileService.GetFullPathFileServer(review.Customer.AvatarImagePath)
                    },

                    Address = matchingOrder?.CustomerAddress != null
                        ? new CustomerAddressDTO
                        {
                            Id = matchingOrder.CustomerAddress.Id,
                            FullName = matchingOrder.CustomerAddress.FullName,
                            Phone = matchingOrder.CustomerAddress.Phone,
                            Address = matchingOrder.CustomerAddress.Address,
                            Ward = matchingOrder.CustomerAddress.Ward,
                            District = matchingOrder.CustomerAddress.District,
                            Province = matchingOrder.CustomerAddress.Province,
                        }
                        : null,
                    OrderItems = orderItem != null
                        ? new List<OrderItemDTO>
                        {
                    new OrderItemDTO
                    {
                        ProductId = orderItem.ProductId,
                        ProductName = orderItem.Product?.ProductName,
                        Quantity = orderItem.Quantity,
                        DiscountPrice = orderItem.Product?.DiscountPrice,
                        ImagePath = fileService.GetFullPathFileServer(orderItem.Product?.ImagePath)
                    }
                        }
                        : null
                });
            }

            return result;
        }

    }
}