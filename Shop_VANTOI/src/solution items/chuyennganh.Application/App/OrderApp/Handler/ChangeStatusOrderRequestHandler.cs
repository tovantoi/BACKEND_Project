using AutoMapper;
using chuyennganh.Application.App.OrderApp.Command;
using chuyennganh.Application.App.OrderApp.Validators;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.OrderApp.Handler
{
    public class ChangeStatusOrderRequestHandler : IRequestHandler<ChangeStatusOrderRequest, ServiceResponse>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public ChangeStatusOrderRequestHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
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

                    order!.Status = request.Status ?? order.Status;

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
