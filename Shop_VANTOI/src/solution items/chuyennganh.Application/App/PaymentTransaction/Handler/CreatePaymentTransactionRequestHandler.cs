using AutoMapper;
using chuyennganh.Application.App.PaymentTransaction.Command;
using chuyennganh.Application.App.PaymentTransaction.Validators;
using chuyennganh.Application.Repositories;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace chuyennganh.Application.App.PaymentTransaction.Handler
{
    public class CreatePaymentTransactionRequestHandler : IRequestHandler<CreatePaymentTransactionRequest, ServiceResponse>
    {
        private readonly IPaymentTransactionRepository paymentTransactionRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IPaymentService paymentService;

        public CreatePaymentTransactionRequestHandler(
            IPaymentTransactionRepository paymentTransactionRepository,
            IMapper mapper,
            IOrderRepository orderRepository,
            IPaymentService paymentService)
        {
            this.paymentTransactionRepository = paymentTransactionRepository;
            this.mapper = mapper;
            this.orderRepository = orderRepository;
            this.paymentService = paymentService;
        }

        public async Task<ServiceResponse> Handle(CreatePaymentTransactionRequest request, CancellationToken cancellationToken)
        {
            await using var transaction = paymentTransactionRepository.BeginTransaction();
            bool isCommitted = false;

            try
            {
                var validator = new CreatePaymentTransactionValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return ServiceResponse.Failure("Dữ liệu không hợp lệ.");
                }

                if (!await orderRepository.ExistsAsync(request.OrderId))
                {
                    return ServiceResponse.Failure("Đơn hàng không tồn tại.");
                }

                var paymentTransaction = mapper.Map<Domain.Entities.PaymentTransaction>(request);
                paymentTransaction.Status = "Pending";
                paymentTransaction.TransactionId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

                paymentTransactionRepository.Create(paymentTransaction);
                await paymentTransactionRepository.SaveChangeAsync();

                await transaction.CommitAsync(cancellationToken);
                isCommitted = true;

                // Gọi tạo link thanh toán PayPal
                var paymentUrl = await paymentService.CreatePayPalPayment(
                    request.Amount,
                    request.OrderId.ToString(),
                    request.ReturnUrl ?? "https://c188-115-79-36-134.ngrok-free.app/payment-success"
                );

                return ServiceResponse.Success("Tạo giao dịch thành công", query: new { paymentUrl });
            }
            catch (Exception e)
            {
                if (!isCommitted)
                {
                    try
                    {
                        await transaction.RollbackAsync(cancellationToken);
                    }
                    catch
                    {
                        // Bỏ qua lỗi rollback nếu đã hoàn thành
                    }
                }

                return new ServiceResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = e.Message
                };
            }
        }
    }
}
