using chuyennganh.Application.App.Thong_Ke.Command;
using chuyennganh.Application.App.Thong_Ke.Dto;
using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Domain.Enumerations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace chuyennganh.Application.App.Thong_Ke.Handler
{
    public class GetSalesSummaryRequestHandler : IRequestHandler<GetSalesSummaryRequest, SalesSummaryDTO>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICustomerRepository customerRepository;

        public GetSalesSummaryRequestHandler(IOrderRepository orderRepository, ICustomerRepository customerRepository)
        {
            this.orderRepository = orderRepository;
            this.customerRepository = customerRepository;
        }

        public async Task<SalesSummaryDTO> Handle(GetSalesSummaryRequest request, CancellationToken cancellationToken)
        {
            var from = request.From ?? DateTime.MinValue;
            var to = request.To ?? DateTime.MaxValue;

            var orders = await orderRepository.FindAll()
                .Where(o => o.Status == OrderStatus.Successed &&
                            o.CreatedAt >= from && o.CreatedAt <= to)
                .Include(o => o.OrderItems)
                .Include(o => o.Customer)
                .ToListAsync(cancellationToken);

            int totalProductsSold = orders
                .SelectMany(o => o.OrderItems!)
                .Sum(oi => oi.Quantity ?? 0);

            // Lọc khách hàng mới: tạo trong thời gian này
            var newCustomerIds = orders
                .Where(o => o.Customer != null && o.Customer.CreatedAt >= from && o.Customer.CreatedAt <= to)
                .Select(o => o.CustomerId)
                .Distinct()
                .ToList();

            decimal totalRevenueFromNewCustomers = orders
                .Where(o => newCustomerIds.Contains(o.CustomerId))
                .Sum(o => o.TotalPrice ?? 0);

            // Chi phí giả định = 60% doanh thu
            decimal estimatedCost = totalRevenueFromNewCustomers * 0.6m;

            return new SalesSummaryDTO
            {
                TotalProductsSold = totalProductsSold,
                TotalRevenueFromNewCustomers = totalRevenueFromNewCustomers,
                EstimatedCost = estimatedCost
            };
        }
    }

}
