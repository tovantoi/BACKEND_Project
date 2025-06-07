using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Infrasture.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.OrderRepo
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly AppDbContext dbContext;

        public OrderRepository(AppDbContext dbContext, ILogger<GenericRepository<Order>> logger) : base(dbContext, logger)
        {
            this.dbContext = dbContext; // ✅ QUAN TRỌNG: thêm dòng này
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            try
            {
                var order = await dbContext.Orders
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                    return false;

                order.Status = newStatus;
                order.UpdatedAt = DateTime.Now;

                dbContext.Orders.Update(order);
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // ✅ Log lỗi nếu cần
                Console.WriteLine("❌ Lỗi khi cập nhật đơn hàng: " + ex.Message);
                return false;
            }
        }
    }
}
