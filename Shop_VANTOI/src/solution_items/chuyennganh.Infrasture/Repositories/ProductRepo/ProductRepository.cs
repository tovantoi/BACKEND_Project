using chuyennganh.Application.Repositories.ProductRepo;
using Entities = chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using chuyennganh.Application.App.ProductApp.Command;

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
        public async Task<List<ProductInventoryDto>> GetProductInventoryAsync()
        {
            return await dbContext.Products
                .Select(p => new ProductInventoryDto
                {
                    ProductId = p.Id ?? 0,
                    ProductName = p.ProductName,
                    TotalStock = p.ProductSizes.Any()
                        ? p.ProductSizes.Sum(s => s.StockQuantity)
                        : (p.StockQuantity ?? 0),
                    Sizes = p.ProductSizes.Select(s => new SizeStockDto
                    {
                        SizeLabel = s.SizeLabel,
                        StockQuantity = s.StockQuantity
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}
