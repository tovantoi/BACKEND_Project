using AutoMapper;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.ProductRepo
{
    public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(AppDbContext dbContext, ILogger<GenericRepository<ProductCategory>> logger) : base(dbContext, logger)
        {
        }
    }
}
