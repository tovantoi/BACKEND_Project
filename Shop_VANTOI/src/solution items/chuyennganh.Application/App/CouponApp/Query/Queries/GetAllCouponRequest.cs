using chuyennganh.Domain.Entities;
using MediatR;
using Entities = chuyennganh.Domain.Entities;

namespace chuyennganh.Application.App.CouponApp.Query.Queries
{
    public class GetAllCouponRequest : IRequest<List<Entities.Coupon>>
    {
    }
}
