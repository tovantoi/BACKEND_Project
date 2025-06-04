using AutoMapper;
using chuyennganh.Application.App.OrderApp.Command;
using chuyennganh.Application.App.OrderApp.Validators;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.OrderApp.Handler
{
    public class ChangeStatusOrderUserHandler : IRequestHandler<ChangeStatusOrderUserRequest, ServiceResponse>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public ChangeStatusOrderUserHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<ServiceResponse> Handle(ChangeStatusOrderUserRequest request, CancellationToken cancellationToken)
        {
            await using var transaction = orderRepository.BeginTransaction();
            try
            {
                var validator = new ChangeStatusOrderUserValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return ServiceResponse.Failure(errorMessage);
                }

                var order = await orderRepository.GetByIdAsync(request.Id!.Value);
                if (order is null) order.ThrowNotFound();

                // Xử lý riêng nếu trạng thái yêu cầu là Canceled
                if (request.Status == OrderStatus.Canceled)
                {
                    if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Accepted)
                    {
                        return ServiceResponse.Failure("Không thể huỷ đơn hàng ở trạng thái hiện tại.");
                    }
                }

                order.Status = request.Status ?? order.Status;

                await orderRepository.UpdateAsync(order);
                await orderRepository.SaveChangeAsync();
                await transaction.CommitAsync(cancellationToken);

                return ServiceResponse.Success("Cập nhật trạng thái thành công.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

    }
}
