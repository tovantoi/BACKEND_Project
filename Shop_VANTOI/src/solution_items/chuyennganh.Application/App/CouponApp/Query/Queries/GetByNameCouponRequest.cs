using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.CouponApp.Query.Queries
{
    public class GetByNameCouponRequest : IRequest<Coupon>
    {
        public string? Code { get; set; }
    }
}
