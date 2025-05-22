using chuyennganh.Domain.Entities;
using MediatR;

namespace chuyennganh.Application.App.CouponApp.Query.Queries
{
    public class GetByIdCouponRequest : IRequest<Coupon>
    {
        public int? Id { get; set; }
    }
}
