using AutoMapper;
using chuyennganh.Application.App.CustomersApp.CustomerAddress.Query.Queries;
using chuyennganh.Application.App.CustomersApp.Validators;
using chuyennganh.Application.App.DTOs;
using chuyennganh.Application.Repositories.CustomerAddressRPRepo;
using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.CustomersApp.CustomerAddress.Query.Handlers
{
    public class GetCustomerAddressByCustomerIdRequestHandler : IRequestHandler<GetCustomerAddressByCustomerIdRequest, List<CustomerAddressDTO>>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ICustomerAddressRepository customerAddressRepository;
        private readonly IMapper mapper;

        public GetCustomerAddressByCustomerIdRequestHandler(ICustomerRepository customerRepository, ICustomerAddressRepository customerAddressRepository, IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.customerAddressRepository = customerAddressRepository;
            this.mapper = mapper;
        }

        public async Task<List<CustomerAddressDTO>> Handle(GetCustomerAddressByCustomerIdRequest request, CancellationToken cancellationToken)
        {
            var validator = new GetCustomerAddressByCustomerIdRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            var customer = await customerRepository.GetByIdAsync(request.CustomerId!);
            if (customer is null) customer.ThrowNotFound();

            var customeAaddress = customerAddressRepository.FindAll(x => x.CustomerId == request.CustomerId).ToList();
            if (customeAaddress is null) customeAaddress.ThrowNotFound();

            return mapper.Map<List<CustomerAddressDTO>>(customeAaddress);
        }
    }
}