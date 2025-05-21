using chuyennganh.Application.App.Thong_Ke.Dto;
using MediatR;

namespace chuyennganh.Application.App.Thong_Ke.Command
{
    public class GetCategoryRevenueRequest : IRequest<List<CategoryRevenueDTO>>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
