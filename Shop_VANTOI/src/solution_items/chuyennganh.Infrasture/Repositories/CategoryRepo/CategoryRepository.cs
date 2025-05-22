using chuyennganh.Application.Repositories.CategoryRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.CategoryRepo
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext, ILogger<GenericRepository<Category>> logger) : base(dbContext, logger)
        {
        }
    }
}
