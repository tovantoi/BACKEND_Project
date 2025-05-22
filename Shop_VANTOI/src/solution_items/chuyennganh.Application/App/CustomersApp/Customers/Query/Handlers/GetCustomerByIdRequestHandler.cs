using AutoMapper;
using chuyennganh.Application.App.CustomersApp.Customers.Query.Queries;
using chuyennganh.Application.Repositories.CategoryRepo;
using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.ExceptionEx;
using chuyennganh.Domain.Services;
using MediatR;

namespace chuyennganh.Application.App.CustomersApp.Customers.Query.Handlers
{
    public class GetCustomerByIdRequestHandler : IRequestHandler<GetCustomerByIdCustomerRequest, Customer>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        public GetCustomerByIdRequestHandler(ICustomerRepository customerRepository, IMapper mapper, IFileService fileService)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<Customer> Handle(GetCustomerByIdCustomerRequest request, CancellationToken cancellationToken)
        {
            Customer? customers = await customerRepository.GetByIdAsync(request.Id);
            if (customers != null)
            {
                customers.AvatarImagePath = fileService.GetFullPathFileServer(customers.AvatarImagePath); // Adding EmpImage property
            }
            if (customers is null) customers.ThrowNotFound();
            return mapper.Map<Customer>(customers);
        }
    }
}
