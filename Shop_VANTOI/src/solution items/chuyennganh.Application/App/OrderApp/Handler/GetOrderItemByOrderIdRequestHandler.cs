//using AutoMapper;
//using chuyennganh.Application.Repositories.OrderItemRepo;
//using chuyennganh.Application.Repositories.OrderRepo;
//using MediatR;
//using NhaThuoc.Application.Request.Order;
//using NhaThuoc.Domain.Entities;
//using NhaThuoc.Share.DependencyInjection.Extensions;

//namespace NhaThuoc.Application.Handlers.Order
//{
//    public class GetOrderItemByOrderIdRequestHandler : IRequestHandler<GetOrderItemByOrderIdRequest, List<OrderItem>>
//    {
//        private readonly IOrderItemRepository orderitemRepository;
//        private readonly IOrderRepository orderRepository;
//        private readonly IMapper mapper;

//        public GetOrderItemByOrderIdRequestHandler(IOrderItemRepository orderitemRepository, IMapper mapper, IOrderRepository orderRepository)
//        {
//            this.orderitemRepository = orderitemRepository;
//            this.mapper = mapper;
//            this.orderRepository = orderRepository;
//        }

//        public async Task<List<OrderItem>> Handle(GetOrderItemByOrderIdRequest request, CancellationToken cancellationToken)
//        {
//            var order = await orderRepository.FindByIdAsync(request.OrderId!);
//            if (order is null) order.ThrowNotFound();
//            var orderItem = orderitemRepository.FindAll(x => x.OrderId == request.OrderId).ToList();
//            return mapper.Map<List<OrderItem>>(orderItem);
//        }
//    }
//}
