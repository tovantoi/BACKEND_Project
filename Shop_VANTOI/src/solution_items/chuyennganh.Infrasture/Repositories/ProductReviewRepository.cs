using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.OrderRepo
{
    public class ProductReviewRepository : GenericRepository<ProductReview>, IProductReviewRepository
    {
        private readonly AppDbContext dbContext;
        public ProductReviewRepository(AppDbContext dbContext, ILogger<GenericRepository<ProductReview>> logger) : base(dbContext, logger)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> HasUserReviewedProductAsync(int userId, int productId)
        {
            return await dbContext.ProductReviews
                .AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }

    }
}
