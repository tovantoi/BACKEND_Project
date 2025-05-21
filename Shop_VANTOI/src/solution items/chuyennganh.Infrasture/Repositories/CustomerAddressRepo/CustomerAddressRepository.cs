using chuyennganh.Application.Repositories.CustomerAddressRPRepo;
using chuyennganh.Domain.Entities;
using chuyennganh.Infrasture.Context;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Infrasture.Repositories.CustomerAddressRepo
{
    public class CustomerAddressRepository : GenericRepository<CustomerAddress>, ICustomerAddressRepository
    {
        public CustomerAddressRepository(AppDbContext dbContext, ILogger<GenericRepository<CustomerAddress>> logger) : base(dbContext, logger)
        {
        }
    }
}
