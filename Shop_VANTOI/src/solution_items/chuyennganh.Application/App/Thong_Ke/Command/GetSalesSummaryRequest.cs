using chuyennganh.Application.App.Thong_Ke.Dto;
using MediatR;

namespace chuyennganh.Application.App.Thong_Ke.Command
{
    public class GetSalesSummaryRequest : IRequest<SalesSummaryDTO>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

}
