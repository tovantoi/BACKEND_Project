using chuyennganh.Application.App.Thong_Ke.Dto;
using MediatR;

namespace chuyennganh.Application.App.Thong_Ke.Command
{
    public class GetRevenueStatisticsRequest : IRequest<List<RevenueStatisticsDTO>>
    {
        public string? Mode { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
