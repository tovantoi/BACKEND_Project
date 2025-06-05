using chuyennganh.Application.Repositories.ProductRepo;
using Entities = chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace chuyennganh.Infrasture.Repositories.ProductRepo
{
    public class ProductRepository : GenericRepository<Entities.Product>, IProductRepository
    {
        private readonly AppDbContext dbContext;

        public ProductRepository(AppDbContext dbContext, ILogger<GenericRepository<Entities.Product>> logger) : base(dbContext, logger)
        {
            this.dbContext = dbContext;
        }

        public async Task<Entities.Product?> GetByIdWithImagesAsync(int id)
        {
            return await dbContext.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
