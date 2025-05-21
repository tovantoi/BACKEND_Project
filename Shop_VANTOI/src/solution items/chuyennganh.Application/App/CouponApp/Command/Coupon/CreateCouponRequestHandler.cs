using AutoMapper;
using chuyennganh.Application.App.CouponApp.Validators;
using chuyennganh.Application.Repositories.CouponRepo;
using chuyennganh.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace chuyennganh.Application.App.CouponApp.Command.Coupon
{
    public class CreateCouponRequestHandler : IRequestHandler<CouponCreateRequest, ServiceResponse>
    {
        private readonly ICouponRepository couponRepository;
        private readonly IMapper mapper;
        public CreateCouponRequestHandler(ICouponRepository couponRepository, IMapper mapper)
        {
            this.couponRepository = couponRepository;
            this.mapper = mapper;
        }
        public async Task<ServiceResponse> Handle(CouponCreateRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = couponRepository.BeginTransaction())
            {
                try
                {
                    var validator = new CreateCouponRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var coupon = mapper.Map<Domain.Entities.Coupon>(request);
                    coupon.Code!.ToUpper();

                    await couponRepository.AddAsync(coupon);
                    await couponRepository.SaveChangeAsync();
                    await transaction.CommitAsync(cancellationToken);
                    return ServiceResponse.Success("Tạo thành công");
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new ServiceResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                    };
                }
            }
        }
    }
}
