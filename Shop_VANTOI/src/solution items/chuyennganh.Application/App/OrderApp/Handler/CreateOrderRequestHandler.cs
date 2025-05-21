using AutoMapper;
using chuyennganh.Application.App.OrderApp.Command;
using chuyennganh.Application.App.OrderApp.Validators;
using chuyennganh.Application.Repositories.CouponRepo;
using chuyennganh.Application.Repositories.OrderItemRepo;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.ExceptionEx;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace chuyennganh.Application.App.OrderApp.Handler
{
    public class CreateOrderRequestHandler : IRequestHandler<CreateOrderRequest, ServiceResponse>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly ICouponRepository couponRepository;
        private readonly IProductRepository productRepository;
        private readonly IOrderItemRepository orderItemRepository;

        public CreateOrderRequestHandler(IOrderRepository orderRepository, IMapper mapper, ICouponRepository couponRepository, IProductRepository productRepository, IOrderItemRepository orderItemRepository)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.couponRepository = couponRepository;
            this.productRepository = productRepository;
            this.orderItemRepository = orderItemRepository;
        }

        public async Task<ServiceResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = orderRepository.BeginTransaction())
            {
                try
                {
                    var validator = new CreateOrderRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    Coupon coupon = null!;

                    var order = new Order
                    {
                        CustomerId = request.CustomerId,
                        CustomerAddressId = request.CustomerAddressId,
                        Payment = request.PaymentMethod,
                        Status = OrderStatus.Pending,
                        TotalPrice = 0,
                        CreatedAt = DateTime.Now,
                    };

                    if (request.CouponCode is not null)
                    {
                        coupon = await couponRepository.FindSingleAsync(x => x.Code == request.CouponCode && x.IsActive && x.TimesUsed < x.MaxUsage && x.CouponEndDate >= DateTime.Now);
                        if (coupon is null) coupon.ThrowNotFound("Mã giảm giá không hợp lệ.");
                        order.CouponId = coupon!.Id;
                    }

                    await orderRepository.AddAsync(order);
                    await orderRepository.SaveChangeAsync();

                    decimal orderTotalPrice = 0;

                    foreach (var item in request.OrderItems!)
                    {
                        var product = await productRepository.GetByIdAsync(item.ProductId!);
                        if (product is null) product.ThrowNotFound();

                        decimal price = product!.DiscountPrice.HasValue && product.DiscountPrice > 0 ? (decimal)product.DiscountPrice.Value : (decimal)(product.RegularPrice ?? 0);
                        var orderItem = new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            TotalPrice = (decimal)(price * item.Quantity!),
                        };
                        orderTotalPrice += orderItem.TotalPrice;
                        await orderItemRepository.AddAsync(orderItem);
                    }

                    await orderItemRepository.SaveChangeAsync();
                    if (coupon is not null)
                    {
                        if (!decimal.TryParse(coupon.Discount, out var discount)) throw new FormatException("Giá trị giảm giá không hợp lệ.");
                        orderTotalPrice -= discount;
                        if (orderTotalPrice < 0) orderTotalPrice = 0;

                        coupon.TimesUsed += 1;
                        await couponRepository.UpdateAsync(coupon);
                        await couponRepository.SaveChangeAsync();
                    }

                    order.TotalPrice = (decimal)orderTotalPrice;
                    await orderRepository.UpdateAsync(order);
                    await orderRepository.SaveChangeAsync();

                    await transaction.CommitAsync(cancellationToken);
                    return ServiceResponse.Success("Tạo thành công", query: new { id = order.Id });
                }
                catch (Exception)
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
