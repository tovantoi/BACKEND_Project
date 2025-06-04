using chuyennganh.Application.App.Thong_Ke.Command;
using chuyennganh.Application.App.Thong_Ke.Dto;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace chuyennganh.Application.App.Thong_Ke.Handler
{
    public class GetCurrentRevenueStatisticsRequestHandler : IRequestHandler<GetRevenueStatisticsRequest, List<RevenueStatisticsDTO>>
    {
        private readonly IOrderRepository orderRepository;

        public GetCurrentRevenueStatisticsRequestHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<List<RevenueStatisticsDTO>> Handle(GetRevenueStatisticsRequest request, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var mode = request.Mode?.ToLower();
            DateTime from = DateTime.MinValue;
            DateTime to = DateTime.MaxValue;

            switch (mode)
            {
                case "day":
                    from = now.Date;
                    to = now.Date.AddDays(1).AddTicks(-1);
                    break;
                case "week":
                    var diff = now.DayOfWeek - DayOfWeek.Monday;
                    from = now.AddDays(-1 * (diff < 0 ? 6 : diff)).Date;
                    to = from.AddDays(7).AddTicks(-1);
                    break;
                case "month":
                    from = new DateTime(now.Year, now.Month, 1);
                    to = from.AddMonths(1).AddTicks(-1);
                    break;
                case "year":
                    from = new DateTime(now.Year, 1, 1);
                    to = from.AddYears(1).AddTicks(-1);
                    break;
                default:
                    throw new ArgumentException("Mode không hợp lệ. Chọn 'day', 'week', 'month', hoặc 'year'.");
            }

            var orders = await orderRepository.FindAll()
                .Where(o => o.Status == OrderStatus.Successed && o.CreatedAt >= from && o.CreatedAt <= to)
                .ToListAsync(cancellationToken);

            var totalRevenue = orders.Sum(x => x.TotalPrice ?? 0);

            return new List<RevenueStatisticsDTO>
        {
            new RevenueStatisticsDTO
            {
                Label = $"{mode.ToUpper()} hiện tại",
                TotalRevenue = totalRevenue,
                Type = mode
            }
        };
        }
    }

}
