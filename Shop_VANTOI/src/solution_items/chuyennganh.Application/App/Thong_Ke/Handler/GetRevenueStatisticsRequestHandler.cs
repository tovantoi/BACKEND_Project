using chuyennganh.Application.App.Thong_Ke.Command;
using chuyennganh.Application.App.Thong_Ke.Dto;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace chuyennganh.Application.App.Thong_Ke.Handler
{
    public class GetRevenueStatisticsRequestHandler : IRequestHandler<GetRevenueStatisticsRequest, List<RevenueStatisticsDTO>>
    {
        private readonly IOrderRepository orderRepository;

        public GetRevenueStatisticsRequestHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<List<RevenueStatisticsDTO>> Handle(GetRevenueStatisticsRequest request, CancellationToken cancellationToken)
        {
            var from = request.From ?? DateTime.MinValue;
            var to = request.To ?? DateTime.MaxValue;

            var orders = await orderRepository.FindAll()
                .Where(o => o.Status == OrderStatus.Successed
                         && o.CreatedAt >= from && o.CreatedAt <= to)
                .ToListAsync(cancellationToken);

            var mode = request.Mode?.ToLower();

            var result = new List<RevenueStatisticsDTO>();

            if (string.IsNullOrEmpty(mode) || mode == "day")
            {
                var byDay = orders
                    .GroupBy(o => o.CreatedAt!.Value.Date)
                    .Select(g => new RevenueStatisticsDTO
                    {
                        Label = g.Key.ToString("yyyy-MM-dd"),
                        TotalRevenue = g.Sum(x => x.TotalPrice ?? 0),
                        Type = "day"
                    });
                result.AddRange(byDay);
            }
            if (string.IsNullOrEmpty(mode) || mode == "week")
            {
                var byWeek = orders
                    .AsEnumerable() // ⚠️ cần thiết để xử lý .NET Calendar
                    .GroupBy(o =>
                        CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                            o.CreatedAt!.Value,
                            CalendarWeekRule.FirstFourDayWeek,
                            DayOfWeek.Monday))
                    .Select(g => new RevenueStatisticsDTO
                    {
                        Label = $"Tuần {g.Key}",
                        TotalRevenue = g.Sum(x => x.TotalPrice ?? 0),
                        Type = "week"
                    });

                result.AddRange(byWeek);
            }

            if (string.IsNullOrEmpty(mode) || mode == "month")
            {
                var byMonth = orders
                    .GroupBy(o => new { o.CreatedAt!.Value.Year, o.CreatedAt!.Value.Month })
                    .Select(g => new RevenueStatisticsDTO
                    {
                        Label = $"Tháng {g.Key.Month}/{g.Key.Year}",
                        TotalRevenue = g.Sum(x => x.TotalPrice ?? 0),
                        Type = "month"
                    });
                result.AddRange(byMonth);
            }

            if (string.IsNullOrEmpty(mode) || mode == "year")
            {
                var byYear = orders
                    .GroupBy(o => o.CreatedAt!.Value.Year)
                    .Select(g => new RevenueStatisticsDTO
                    {
                        Label = g.Key.ToString(),
                        TotalRevenue = g.Sum(x => x.TotalPrice ?? 0),
                        Type = "year"
                    });
                result.AddRange(byYear);
            }

            return result.OrderBy(x => x.Type).ThenBy(x => x.Label).ToList();
        }
    }

}
