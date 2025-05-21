using AutoMapper;
using chuyennganh.Application.App.CustomersApp.Customers.Query.Queries;
using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace chuyennganh.Application.App.CustomersApp.Customers.Query.Handlers
{
    public class GetAllCustomerRequestHandler : IRequestHandler<GetAllCustomerRequest, List<Customer>>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public GetAllCustomerRequestHandler(ICustomerRepository customerRepository, IMapper mapper, IFileService fileService)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<List<Customer>> Handle(GetAllCustomerRequest request, CancellationToken cancellationToken)
        {
            var customers = await customerRepository
                                 .FindAll(u => u.Role == 0)
                                 .ToListAsync(cancellationToken);
            var employees = await Task.Run(() =>
            {
                return customers
                    .Select(c => new
                    {
                        Customers = c,
                        AvatarImagePath = fileService.GetFullPathFileServer(c.AvatarImagePath)
                    })
                    .ToList();
            }, cancellationToken);

            var result = employees
                .Select(x =>
                {
                    x.Customers.AvatarImagePath = x.AvatarImagePath;
                    return x.Customers;
                })
                .ToList();
            return mapper.Map<List<Customer>>(customers);
        }
    }
}
