using AutoMapper;
using chuyennganh.Application.App.CustomersApp.CustomerAddress.Query.Queries;
using chuyennganh.Application.Repositories.CustomerAddressRPRepo;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.ExceptionEx;
using chuyennganh.Domain.Services;
using MediatR;
using Entities = chuyennganh.Domain.Entities;

namespace chuyennganh.Application.App.CustomersApp.CustomerAddress.Query.Handlers
{
    public class GetByIdCustomerAddressRequestHandler : IRequestHandler<GetByIdCustomerAddressRequest, Entities.CustomerAddress>
    {
        private readonly ICustomerAddressRepository customerAddressRepository;
        private readonly IMapper mapper;

        public GetByIdCustomerAddressRequestHandler(ICustomerAddressRepository customerAddressRepository, IMapper mapper)
        {
            this.customerAddressRepository = customerAddressRepository;
            this.mapper = mapper;
        }
        public async Task<Domain.Entities.CustomerAddress> Handle(GetByIdCustomerAddressRequest request, CancellationToken cancellationToken)
        {
            var customeraddress = await customerAddressRepository.GetByIdAsync(request.Id);
            if (customeraddress is null) customeraddress.ThrowNotFound();
            return mapper.Map<Domain.Entities.CustomerAddress>(customeraddress);
        }
    }
}