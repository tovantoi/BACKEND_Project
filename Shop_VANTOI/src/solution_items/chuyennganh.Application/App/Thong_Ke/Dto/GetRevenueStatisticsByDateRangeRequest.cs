using MediatR;

namespace chuyennganh.Application.App.Thong_Ke.Dto
{
    public class GetRevenueStatisticsByDateRangeRequest : IRequest<List<RevenueStatisticsDTO>>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
