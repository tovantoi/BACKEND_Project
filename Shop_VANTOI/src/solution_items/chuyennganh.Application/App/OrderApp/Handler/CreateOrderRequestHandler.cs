using AutoMapper;
using chuyennganh.Application.App.OrderApp.Command;
using chuyennganh.Application.App.OrderApp.Validators;
using chuyennganh.Application.App.SendOrderEmail.Command;
using chuyennganh.Application.Repositories.CouponRepo;
using chuyennganh.Application.Repositories.CustomerRPRepo;
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
        private readonly IMediator mediator;
        private readonly ICustomerRepository customerRepository;
        public CreateOrderRequestHandler(IOrderRepository orderRepository, IMapper mapper, ICouponRepository couponRepository, IProductRepository productRepository, IOrderItemRepository orderItemRepository, IMediator mediator, ICustomerRepository customerRepository)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.couponRepository = couponRepository;
            this.productRepository = productRepository;
            this.orderItemRepository = orderItemRepository;
            this.mediator = mediator;
            this.customerRepository = customerRepository;
        }

        public async Task<ServiceResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            await using var transaction = orderRepository.BeginTransaction();
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
                    coupon = await couponRepository.FindSingleAsync(x =>
                        x.Code == request.CouponCode &&
                        x.IsActive &&
                        x.TimesUsed < x.MaxUsage &&
                        x.CouponEndDate >= DateTime.Now);

                    if (coupon is null)
                        coupon.ThrowNotFound("Mã giảm giá không hợp lệ.");

                    order.CouponId = coupon.Id;
                }

                await orderRepository.AddAsync(order);
                await orderRepository.SaveChangeAsync();

                decimal orderTotalPrice = 0;

                foreach (var item in request.OrderItems!)
                {
                    var product = await productRepository.GetByIdAsync(item.ProductId!);
                    if (product is null) product.ThrowNotFound();

                    decimal price = product.DiscountPrice.HasValue && product.DiscountPrice > 0
                        ? (decimal)product.DiscountPrice.Value
                        : (decimal)(product.RegularPrice ?? 0);

                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        TotalPrice = price * item.Quantity!.Value,
                    };

                    orderTotalPrice += orderItem.TotalPrice;
                    await orderItemRepository.AddAsync(orderItem);
                }

                await orderItemRepository.SaveChangeAsync();

                if (coupon is not null)
                {
                    if (!decimal.TryParse(coupon.Discount, out var discount))
                        throw new FormatException("Giá trị giảm giá không hợp lệ.");

                    orderTotalPrice -= discount;
                    if (orderTotalPrice < 0) orderTotalPrice = 0;

                    coupon.TimesUsed += 1;
                    await couponRepository.UpdateAsync(coupon);
                    await couponRepository.SaveChangeAsync();
                }

                // Cộng thêm phí ship mặc định
                const decimal shippingFee = 30000;
                orderTotalPrice += shippingFee;

                order.TotalPrice = orderTotalPrice;

                var customer = await customerRepository.GetByIdAsync(request.CustomerId);
                if (customer == null) throw new Exception("Không tìm thấy khách hàng.");

                await orderRepository.UpdateAsync(order);
                await orderRepository.SaveChangeAsync();

                await transaction.CommitAsync(cancellationToken);

                await mediator.Send(new SendOrderEmailCommand
                {
                    Email = customer.Email,
                    CustomerName = $"{customer.LastName ?? ""} {customer.FirstName ?? ""}".Trim(),
                    OrderCode = $"OD{order.Id:D6}",
                    TotalPrice = order.TotalPrice ?? 0,
                    OrderItems = request.OrderItems.Select(x => new OrderItemDto
                    {
                        ProductName = productRepository.GetByIdAsync(x.ProductId).Result?.ProductName ?? "Sản phẩm",
                        Quantity = x.Quantity ?? 0,
                        Price = (x.Quantity ?? 0) * (decimal)(productRepository.GetByIdAsync(x.ProductId).Result?.DiscountPrice ?? 0)
                    }).ToList()
                });

                return ServiceResponse.Success("Tạo thành công", query: new { id = order.Id });
            }
            catch (Exception ex)
            {
                try
                {
                    await transaction.RollbackAsync(cancellationToken);
                }
                catch (Exception rollbackEx)
                {
                    // Ghi log nếu rollback cũng lỗi
                    Console.WriteLine("Rollback failed: " + rollbackEx.Message);
                }

                return new ServiceResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = ex.Message
                };
            }
        }

    }
}
