using chuyennganh.Application.App.Thong_Ke.Command;
using chuyennganh.Application.App.Thong_Ke.Dto;
using chuyennganh.Application.Repositories.OrderItemRepo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace chuyennganh.Application.App.Thong_Ke.Handler
{
    public class GetTopProductRevenueRequestHandler : IRequestHandler<GetTopProductRevenueRequest, List<ProductRevenueDTO>>
    {
        private readonly IOrderItemRepository orderItemRepository;

        public GetTopProductRevenueRequestHandler(IOrderItemRepository orderItemRepository)
        {
            this.orderItemRepository = orderItemRepository;
        }

        public async Task<List<ProductRevenueDTO>> Handle(GetTopProductRevenueRequest request, CancellationToken cancellationToken)
        {
            var from = request.From ?? DateTime.MinValue;
            var to = request.To ?? DateTime.MaxValue;

            var result = await orderItemRepository.FindAll()
                .Where(oi => oi.Order != null
                             && oi.Order.CreatedAt >= from
                             && oi.Order.CreatedAt <= to
                             && oi.Order.Status == Domain.Enumerations.OrderStatus.Successed)
                .GroupBy(oi => new { oi.ProductId, oi.Product!.ProductName })
                .Select(g => new ProductRevenueDTO
                {
                    ProductId = g.Key.ProductId ?? 0,
                    ProductName = g.Key.ProductName,
                    TotalSold = g.Sum(x => x.Quantity ?? 0),
                    TotalRevenue = g.Sum(x => x.TotalPrice)
        })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(request.Top)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
