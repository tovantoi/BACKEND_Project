using AutoMapper;
using chuyennganh.Application.App.ProductApp.Command;
using chuyennganh.Application.App.ProductApp.Validators;
using chuyennganh.Application.Repositories.OrderItemRepo;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Enumerations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Application.App.ProductApp.Handler
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, ServiceResponse>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly ILogger<DeleteProductHandler> logger;
        private readonly IOrderItemRepository orderItemRepository;

        public DeleteProductHandler(IProductRepository productRepository, IMapper mapper, ILogger<DeleteProductHandler> logger, IOrderItemRepository orderItemRepository)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.orderItemRepository = orderItemRepository;
        }
        public async Task<ServiceResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var response = new ServiceResponse();
            var validator = new DeleteProductValidator();
            validator.ValidateAndThrow(request);
            await using (var transaction = productRepository.BeginTransaction())
            {
                try
                {
                    var product = await productRepository.GetByIdAsync(request.Id!.Value);
                    if (product == null)
                    {
                        response.IsSuccess = false;
                        response.Message = "Product ID not found";
                        return response;
                    }
                    var hasRestrictedOrders = await orderItemRepository
                        .FindSingleAsync(oi => oi.ProductId == request.Id.Value &&
                                               (oi.Order.Status == OrderStatus.Accepted || oi.Order.Status == OrderStatus.Shipping)); if (product != null)
                    {
                        response.IsSuccess = false;
                        response.Message = "Không thể xóa sản phẩm vì đang có đơn hàng sử dụng.";
                        return response;
                    }
                    await productRepository.DeleteAsync(request.Id.Value);
                    await productRepository.SaveChangeAsync();

                    transaction.Commit();

                    response.IsSuccess = true;
                    response.Message = "Delete Successful";

                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    response.IsSuccess = false;
                    response.Message = $"An error occurred: {ex.Message}";
                    return response;
                }

            }
        }
    }
}
