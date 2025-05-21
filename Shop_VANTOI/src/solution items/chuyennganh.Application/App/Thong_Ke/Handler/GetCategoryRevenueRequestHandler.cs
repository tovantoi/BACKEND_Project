using chuyennganh.Application.App.Thong_Ke.Command;
using chuyennganh.Application.App.Thong_Ke.Dto;
using chuyennganh.Application.Repositories.OrderItemRepo;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace chuyennganh.Application.App.Thong_Ke.Handler
{
    public class GetCategoryRevenueRequestHandler : IRequestHandler<GetCategoryRevenueRequest, List<CategoryRevenueDTO>>
    {
        private readonly IOrderItemRepository orderItemRepository;

        public GetCategoryRevenueRequestHandler(IOrderItemRepository orderItemRepository)
        {
            this.orderItemRepository = orderItemRepository;
        }

        public async Task<List<CategoryRevenueDTO>> Handle(GetCategoryRevenueRequest request, CancellationToken cancellationToken)
        {
            var from = request.From ?? DateTime.MinValue;
            var to = request.To ?? DateTime.MaxValue;

            var query = orderItemRepository.FindAll()
                .Where(oi => oi.Order != null
                             && oi.Order.Status == Domain.Enumerations.OrderStatus.Successed
                             && oi.Order.CreatedAt >= from
                             && oi.Order.CreatedAt <= to
                             && oi.Product != null
                             && oi.Product.ProductCategories != null);

            var result = await query
                .SelectMany(oi => oi.Product.ProductCategories.Select(pc => new
                {
                    pc.CategoryId,
                    CategoryName = pc.Category.Name,
                    Quantity = oi.Quantity ?? 0,
                    Revenue = oi.TotalPrice
                }))
                .GroupBy(x => new { x.CategoryId, x.CategoryName })
                .Select(g => new CategoryRevenueDTO
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName,
                    TotalSold = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Revenue)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
