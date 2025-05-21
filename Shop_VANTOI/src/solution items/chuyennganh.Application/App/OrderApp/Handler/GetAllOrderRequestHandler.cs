using AutoMapper;
using chuyennganh.Application.App.OrderApp.Command;
using chuyennganh.Application.Repositories.OrderRepo;
using MediatR;

namespace chuyennganh.Application.App.OrderApp.Handler
{
    public class GetAllOrderRequestHandler : IRequestHandler<GetAllOrderRequest, List<Domain.Entities.Order>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public GetAllOrderRequestHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<List<Domain.Entities.Order>> Handle(GetAllOrderRequest request, CancellationToken cancellationToken)
        {
            var ordersQuery = orderRepository.FindAll();
            var sortedOrders = ordersQuery
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return mapper.Map<List<Domain.Entities.Order>>(sortedOrders);
        }
    }
}
