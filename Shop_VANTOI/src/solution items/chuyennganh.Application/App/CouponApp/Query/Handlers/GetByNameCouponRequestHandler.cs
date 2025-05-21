using AutoMapper;
using chuyennganh.Application.App.CouponApp.Query.Queries;
using chuyennganh.Application.Repositories.CouponRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.CouponApp.Query.Handlers
{
    public class GetByNameCouponRequestHandler : IRequestHandler<GetByNameCouponRequest, Coupon>
    {
        private readonly ICouponRepository couponRepository;
        private readonly IMapper mapper;

        public GetByNameCouponRequestHandler(IMapper mapper, ICouponRepository couponRepository)
        {
            this.mapper = mapper;
            this.couponRepository = couponRepository;
        }

        public async Task<Coupon> Handle(GetByNameCouponRequest request, CancellationToken cancellationToken)
        {
            // Tìm mã giảm giá theo Code (chỉ trả về mã đầu tiên tìm được)
            var coupon = couponRepository
                .FindAll(x => x.Code!.ToLower() == request.Code!.ToLower())
                .FirstOrDefault();

            // Nếu không tìm thấy, ném lỗi
            if (coupon is null)
                coupon.ThrowNotFound();

            return coupon;
        }
    }
}
