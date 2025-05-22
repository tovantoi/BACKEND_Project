using chuyennganh.Application.Repositories.OrderItemRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.OrderItemRepo
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(AppDbContext dbContext, ILogger<GenericRepository<OrderItem>> logger) : base(dbContext, logger)
        {
        }
    }
}