using AutoMapper;
using chuyennganh.Application.App.CouponApp.Command;
using chuyennganh.Application.App.CouponApp.Validators;
using chuyennganh.Application.Repositories.CouponRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.CouponApp.Handler
{
    public class UpdateCouponRequestHandler : IRequestHandler<CouponUpdateRequest, ServiceResponse>
    {
        private readonly ICouponRepository couponRepository;
        private readonly IMapper mapper;
        public UpdateCouponRequestHandler(ICouponRepository couponRepository, IMapper mapper)
        {
            this.couponRepository = couponRepository;
            this.mapper = mapper;
        }

        public async Task<ServiceResponse> Handle(CouponUpdateRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = couponRepository.BeginTransaction())
            {
                try
                {
                    var validator = new UpdateCouponRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var coupon = await couponRepository.GetByIdAsync(request.Id!);
                    if (coupon is null) coupon.ThrowNotFound();
                    coupon!.Code = request.Code ?? coupon.Code;
                    coupon.Description = request.Description ?? coupon.Description;
                    coupon.TimesUsed = request.TimesUsed ?? coupon.TimesUsed;
                    coupon.MaxUsage = request.MaxUsage ?? coupon.MaxUsage;
                    coupon.Discount = request.Discount ?? coupon.Discount;
                    coupon.IsActive = request.IsActive ?? coupon.IsActive;
                    coupon.CouponEndDate = request.CouponEndDate;

                    await couponRepository.UpdateAsync(coupon);
                    await couponRepository.SaveChangeAsync();
                    await transaction.CommitAsync(cancellationToken);
                    return ServiceResponse.Success("Cập nhật thành công");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}
