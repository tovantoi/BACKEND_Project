using AutoMapper;
using chuyennganh.Application.App.OrderApp.Command;
using chuyennganh.Application.App.OrderApp.Validators;
using chuyennganh.Application.Repositories.OrderItemRepo;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.ExceptionEx;
using chuyennganh.Domain.Enumerations;
using MediatR;

namespace chuyennganh.Application.App.OrderApp.Handler
{
    public class ChangeStatusOrderRequestHandler : IRequestHandler<ChangeStatusOrderRequest, ServiceResponse>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;
        private readonly IOrderItemRepository orderItemRepository;

        public ChangeStatusOrderRequestHandler(
            IOrderRepository orderRepository,
            IMapper mapper,
            IProductRepository productRepository,
            IOrderItemRepository orderItemRepository)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.orderItemRepository = orderItemRepository;
        }

        public async Task<ServiceResponse> Handle(ChangeStatusOrderRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = orderRepository.BeginTransaction())
            {
                try
                {
                    var validator = new ChangeStatusOrderRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var order = await orderRepository.GetByIdAsync(request.Id!);
                    if (order is null) order.ThrowNotFound();

                    // ✅ Trả tồn kho nếu trạng thái chuyển sang Cancelled và đơn chưa huỷ trước đó
                    if (request.Status == OrderStatus.Canceled && order.Status != OrderStatus.Canceled)
                    {
                        var orderItems = await orderItemRepository.FindAllAsync(x => x.OrderId == order.Id);
                        foreach (var item in orderItems)
                        {
                            var product = await productRepository.GetByIdAsync(item.ProductId);
                            if (product != null)
                            {
                                var before = product.StockQuantity ?? 0;
                                var quantityToReturn = item.Quantity ?? 0;
                                product.StockQuantity = (product.StockQuantity ?? 0) + (item.Quantity ?? 0);
                                Console.WriteLine($"[LOG] Sản phẩm {product.ProductName} trước: {before}, trả thêm: {quantityToReturn}, sau: {product.StockQuantity}");

                                await productRepository.UpdateAsync(product);
                            }
                        }
                        await productRepository.SaveChangeAsync();
                    }

                    order.Status = request.Status ?? order.Status;

                    await orderRepository.UpdateAsync(order);
                    await orderRepository.SaveChangeAsync();
                    await transaction.CommitAsync(cancellationToken);

                    return ServiceResponse.Success("Cập nhật trạng thái thành công");
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
