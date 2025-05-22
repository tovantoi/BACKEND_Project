using AutoMapper;
using chuyennganh.Application.App.CustomersApp.CustomerAddress.Query.Queries;
using chuyennganh.Application.Repositories.CustomerAddressRPRepo;
using MediatR;
using Entities = chuyennganh.Domain.Entities;

namespace chuyennganh.Application.App.CustomersApp.CustomerAddress.Query.Handlers
{
    public class GetAllCustomerAddressRequestHandler : IRequestHandler<GetAllCustomerAddressRequest, List<Entities.CustomerAddress>>
    {
        private readonly ICustomerAddressRepository customerAddressRepository;
        private readonly IMapper mapper;

        public GetAllCustomerAddressRequestHandler(ICustomerAddressRepository customerAddressRepository, IMapper mapper)
        {
            this.customerAddressRepository = customerAddressRepository;
            this.mapper = mapper;
        }

        public async Task<List<Entities.CustomerAddress>> Handle(GetAllCustomerAddressRequest request, CancellationToken cancellationToken)
        {
            var customeraddresses = customerAddressRepository.FindAll();

            return mapper.Map<List<Entities.CustomerAddress>>(customeraddresses);
        }
    }
}
