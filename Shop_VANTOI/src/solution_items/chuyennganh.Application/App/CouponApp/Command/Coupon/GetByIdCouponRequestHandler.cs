using AutoMapper;
using chuyennganh.Application.App.CouponApp.Query.Queries;
using chuyennganh.Application.Repositories.CouponRepo;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.CouponApp.Command.Coupon
{
    public class GetByIdCouponRequestHandler : IRequestHandler<GetByIdCouponRequest, Domain.Entities.Coupon>
    {
        private readonly ICouponRepository couponRepository;
        private readonly IMapper mapper;

        public GetByIdCouponRequestHandler(ICouponRepository couponRepository, IMapper mapper)
        {
            this.couponRepository = couponRepository;
            this.mapper = mapper;
        }
        public async Task<Domain.Entities.Coupon> Handle(GetByIdCouponRequest request, CancellationToken cancellationToken)
        {
            var coupon = await couponRepository.GetByIdAsync(request.Id!);
            if (coupon is null) coupon.ThrowNotFound();
            return mapper.Map<Domain.Entities.Coupon>(coupon);
        }
    }
}
