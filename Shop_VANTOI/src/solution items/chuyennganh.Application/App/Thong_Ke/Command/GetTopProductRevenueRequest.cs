using chuyennganh.Application.App.Thong_Ke.Dto;
using MediatR;

namespace chuyennganh.Application.App.Thong_Ke.Command
{
    public class GetTopProductRevenueRequest : IRequest<List<ProductRevenueDTO>>
    {
        public int Top { get; set; } = 10; 
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
