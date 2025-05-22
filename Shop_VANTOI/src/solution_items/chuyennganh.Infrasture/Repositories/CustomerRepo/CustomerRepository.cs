using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.CustomerRepo
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext dbContext, ILogger<GenericRepository<Customer>> logger) : base(dbContext, logger)
        {
        }
    }
}
