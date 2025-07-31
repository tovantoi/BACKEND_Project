using chuyennganh.Application.Repositories.OrderItemRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace chuyennganh.Infrasture.Repositories.OrderItemRepo
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        private readonly AppDbContext dbContext;
        public OrderItemRepository(AppDbContext dbContext, ILogger<GenericRepository<OrderItem>> logger) : base(dbContext, logger)
        {
            this.dbContext = dbContext;
        }
        public async Task<OrderItem?> FindSingleAsync(
       Expression<Func<OrderItem, bool>> predicate,
       Func<IQueryable<OrderItem>, IQueryable<OrderItem>>? include = null)
        {
            IQueryable<OrderItem> query = dbContext.OrderItems;

            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

    }
}