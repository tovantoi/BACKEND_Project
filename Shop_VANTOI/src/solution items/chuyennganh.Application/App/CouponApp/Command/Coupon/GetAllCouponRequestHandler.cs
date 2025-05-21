using AutoMapper;
using chuyennganh.Application.App.CouponApp.Query.Queries;
using chuyennganh.Application.Repositories.CouponRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.CouponApp.Query.Handlers
{
    public class GetAllCouponRequestHandler : IRequestHandler<GetAllCouponRequest, List<Coupon>>
    {
        private readonly ICouponRepository couponRepository;
        private readonly IMapper mapper;

        public GetAllCouponRequestHandler(IMapper mapper, ICouponRepository couponRepository)
        {
            this.couponRepository = couponRepository;
            this.mapper = mapper;
        }

        public async Task<List<Coupon>> Handle(GetAllCouponRequest request, CancellationToken cancellationToken)
        {
            var category = couponRepository.FindAll(x => x.TimesUsed < x.MaxUsage && x.CouponEndDate > DateTime.Now);
            if (category is null) category.ThrowNotFound();
            return mapper.Map<List<Coupon>>(category);
        }
    }
}
