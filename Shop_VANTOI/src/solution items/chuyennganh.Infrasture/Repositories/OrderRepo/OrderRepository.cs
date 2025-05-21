using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.OrderRepo
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext dbContext, ILogger<GenericRepository<Order>> logger) : base(dbContext, logger)
        {

        }
    }
}
