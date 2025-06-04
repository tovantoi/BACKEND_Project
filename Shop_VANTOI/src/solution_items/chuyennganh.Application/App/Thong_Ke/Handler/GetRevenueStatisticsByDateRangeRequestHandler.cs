using chuyennganh.Application.App.Thong_Ke.Dto;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace chuyennganh.Application.App.Thong_Ke.Handler
{
    public class GetRevenueStatisticsByDateRangeRequestHandler : IRequestHandler<GetRevenueStatisticsByDateRangeRequest, List<RevenueStatisticsDTO>>
    {
        private readonly IOrderRepository orderRepository;

        public GetRevenueStatisticsByDateRangeRequestHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<List<RevenueStatisticsDTO>> Handle(GetRevenueStatisticsByDateRangeRequest request, CancellationToken cancellationToken)
        {
            var from = request.From;
            var to = request.To;

            if (from == null || to == null || from > to)
                throw new ArgumentException("Ngày bắt đầu và kết thúc không hợp lệ");

            var endDateExclusive = request.To.Value.Date.AddDays(1);

            var orders = await orderRepository.FindAll()
                .Where(o => o.Status == OrderStatus.Successed
                         && o.CreatedAt >= request.From
                         && o.CreatedAt < endDateExclusive) // Dùng < để bao trọn ngày cuối
                .ToListAsync(cancellationToken);


            var result = orders
                .GroupBy(o => o.CreatedAt!.Value.Date)
                .Select(g => new RevenueStatisticsDTO
                {
                    Label = g.Key.ToString("yyyy-MM-dd"),
                    TotalRevenue = g.Sum(x => x.TotalPrice ?? 0),
                    Type = "custom"
                })
                .OrderBy(x => x.Label)
                .ToList();

            return result;
        }
    }
}
