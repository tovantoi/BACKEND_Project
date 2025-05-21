using chuyennganh.Application.Repositories.CouponRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.CouponRepo
{
    public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(AppDbContext dbContext, ILogger<GenericRepository<Coupon>> logger) : base(dbContext, logger)
        {
        }
    }
}
